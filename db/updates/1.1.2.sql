\set ON_ERROR_STOP 1
\set ECHO all
\encoding UTF8

DO $outer$
DECLARE
    current_version TEXT;
    expected_version TEXT := '1.1.1';
    new_version TEXT := '1.1.2';

BEGIN
    
    SELECT version INTO current_version FROM monitoring_center.version_info;

    IF current_version != expected_version THEN
        RAISE EXCEPTION 'The database version % does not match the expected version %', current_version, expected_version;
    END IF;

    -- На тестовом CONSTRAINT unique_name_version_owner_id именуется корректно, а на проде данное условие именуется как unique_name_version_user_id, что нужно исправить --

    -- Создание таблицы fan_overclocking --

    CREATE TABLE monitoring_center.fan_overclocking
    (
        id uuid NOT NULL DEFAULT uuid_generate_v4(),
        type text NOT NULL CHECK (type in ('TargetSpeed', 'TargetTemperature', 'LinearDependence')),
        target_speed integer,
        min_target_speed integer,
        max_target_speed integer,
        target_core_temperature integer,
        target_memory_temperature integer,
        target_points jsonb,
        
        CONSTRAINT pk_fan_overclocking PRIMARY KEY (id)
    );

    INSERT INTO monitoring_center.fan_overclocking(id, type, target_speed)
    SELECT ov.id, 'TargetSpeed', ov.fan_speed
    FROM monitoring_center.overclocking ov
	WHERE ov.target_device_type != 'CPU';

    ALTER TABLE monitoring_center.overclocking
    DROP COLUMN fan_speed,
    ADD COLUMN fan_overclocking_id uuid;

    UPDATE monitoring_center.overclocking o
    SET fan_overclocking_id = fo.id
    FROM monitoring_center.fan_overclocking fo
    WHERE fo.id = o.id;

    ALTER TABLE monitoring_center.overclocking
    ADD CONSTRAINT fk_overclocking_fan_overclocking_fan_overclocking_id FOREIGN KEY (fan_overclocking_id)
    REFERENCES monitoring_center.fan_overclocking (id) MATCH SIMPLE
    	ON DELETE CASCADE
    	ON UPDATE CASCADE;


    -- Обновить функцию организации пресетов при удалении --

    EXECUTE $inner$

        CREATE OR REPLACE FUNCTION monitoring_center.handle_preset_delete()
        RETURNS TRIGGER AS $func$
        DECLARE
            new_preset_id uuid;
            new_overclocking_id uuid;
            mining_device_id uuid;
        BEGIN
            SELECT p.id, p.overclocking_id
            INTO new_preset_id, new_overclocking_id
            FROM monitoring_center.presets p
            WHERE p.name = OLD.id::text
            LIMIT 1;

            FOR mining_device_id IN
                SELECT md.id
                FROM monitoring_center.mining_devices md
                WHERE md.preset_id = OLD.id
            LOOP
                SELECT p.id
                INTO new_preset_id
                FROM monitoring_center.presets p
                WHERE p.name = mining_device_id::text
                LIMIT 1;

                UPDATE monitoring_center.mining_devices
                SET preset_id = new_preset_id
                WHERE id = mining_device_id;

                SELECT p.overclocking_id
                INTO new_overclocking_id
                FROM monitoring_center.presets p
                WHERE p.id = new_preset_id;

                UPDATE monitoring_center.overclocking AS target
                SET 
                    core_clock_lock = source.core_clock_lock,
                    core_clock_offset = source.core_clock_offset,
                    memory_clock_lock = source.memory_clock_lock,
                    memory_clock_offset = source.memory_clock_offset,
                    core_voltage = source.core_voltage,
                    core_voltage_offset = source.core_voltage_offset,
                    memory_voltage = source.memory_voltage,
                    memory_voltage_offset = source.memory_voltage_offset,
                    power_limit = source.power_limit
                FROM monitoring_center.overclocking AS source
                WHERE target.id = new_overclocking_id AND source.id = OLD.overclocking_id;

                UPDATE monitoring_center.fan_overclocking AS target
                SET
                    type = source.type,
                    target_speed = source.target_speed,
                    min_target_speed = source.min_target_speed,
                    max_target_speed = source.max_target_speed,
                    target_core_temperature = source.target_core_temperature,
                    target_memory_temperature = source.target_memory_temperature,
                    target_points = source.target_points
				FROM monitoring_center.overclocking AS tgt_ov,
					 monitoring_center.overclocking AS src_ov,
					 monitoring_center.fan_overclocking AS source
				WHERE tgt_ov.id = new_overclocking_id
				  AND src_ov.id = OLD.overclocking_id
				  AND target.id = tgt_ov.fan_overclocking_id
				  AND source.id = src_ov.fan_overclocking_id;
            END LOOP;

            RETURN OLD;
        END;
        $func$ LANGUAGE plpgsql;

    $inner$;

    -- Установить новую версию бд --

    UPDATE monitoring_center.version_info SET version = new_version;

END
$outer$ LANGUAGE plpgsql;