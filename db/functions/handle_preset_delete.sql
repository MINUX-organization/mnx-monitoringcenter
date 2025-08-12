CREATE OR REPLACE FUNCTION monitoring_center.handle_preset_delete()
RETURNS TRIGGER AS $$
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
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_before_delete_preset
BEFORE DELETE ON monitoring_center.presets
FOR EACH ROW
EXECUTE FUNCTION monitoring_center.handle_preset_delete();