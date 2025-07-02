\set ON_ERROR_STOP 1
\set ECHO all
\encoding UTF8

DO $$
DECLARE
    current_version TEXT;
    expected_version TEXT := '1.1.0';
    new_version TEXT := '1.1.1';

BEGIN
    
    SELECT version INTO current_version FROM monitoring_center.version_info;

    IF current_version != expected_version THEN
        RAISE EXCEPTION 'The database version % does not match the expected version %', current_version, expected_version;
    END IF;

    -- Модификация таблицы monitoring_center.gpu --

    ALTER TABLE monitoring_center.gpu

        -- Исключение ограничений NotNull для параметров ограничений nvidia-разгона таблицы gpu --

        ALTER COLUMN  restrictions_clock_core_lock_is_writable       DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_core_lock_maximal           DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_core_lock_minimal           DROP NOT NULL,

        ALTER COLUMN  restrictions_clock_core_offset_is_writable     DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_core_offset_maximal         DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_core_offset_minimal         DROP NOT NULL,

        ALTER COLUMN  restrictions_clock_memory_lock_is_writable     DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_memory_lock_maximal         DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_memory_lock_minimal         DROP NOT NULL,

        ALTER COLUMN  restrictions_clock_memory_offset_is_writable   DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_memory_offset_maximal       DROP NOT NULL,
        ALTER COLUMN  restrictions_clock_memory_offset_minimal       DROP NOT NULL,

        ALTER COLUMN  restrictions_fan_speed_is_writable             DROP NOT NULL,
        ALTER COLUMN  restrictions_fan_speed_maximal                 DROP NOT NULL,
        ALTER COLUMN  restrictions_fan_speed_minimal                 DROP NOT NULL,

        ALTER COLUMN  restrictions_power_is_writable                 DROP NOT NULL,
        ALTER COLUMN  restrictions_power_maximal                     DROP NOT NULL,
        ALTER COLUMN  restrictions_power_minimal                     DROP NOT NULL,

        ALTER COLUMN  restrictions_temperature_core_is_writable      DROP NOT NULL,
        ALTER COLUMN  restrictions_temperature_core_maximal          DROP NOT NULL,
        ALTER COLUMN  restrictions_temperature_core_minimal          DROP NOT NULL,

        ALTER COLUMN  restrictions_temperature_memory_is_writable    DROP NOT NULL,
        ALTER COLUMN  restrictions_temperature_memory_maximal        DROP NOT NULL,
        ALTER COLUMN  restrictions_temperature_memory_minimal        DROP NOT NULL,

        ALTER COLUMN  restrictions_voltage_core_lock_is_writable     DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_core_lock_maximal         DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_core_lock_minimal         DROP NOT NULL,

        ALTER COLUMN  restrictions_voltage_core_offset_is_writable   DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_core_offset_maximal       DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_core_offset_minimal       DROP NOT NULL,

        ALTER COLUMN  restrictions_voltage_memory_lock_is_writable   DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_memory_lock_maximal       DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_memory_lock_minimal       DROP NOT NULL,

        ALTER COLUMN  restrictions_voltage_memory_offset_is_writable DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_memory_offset_maximal     DROP NOT NULL,
        ALTER COLUMN  restrictions_voltage_memory_offset_minimal     DROP NOT NULL;


    ALTER TABLE monitoring_center.gpu

        -- Добавить параметры ограничений amd-разгона в таблицу gpu --

        ADD COLUMN amd_clock_core_lock_minimal               integer,
        ADD COLUMN amd_clock_core_lock_maximal               integer,
        ADD COLUMN amd_clock_core_lock_default               integer,
        ADD COLUMN amd_clock_core_lock_is_writable           boolean,
        ADD COLUMN amd_clock_core_state_minimal              integer,
        ADD COLUMN amd_clock_core_state_maximal              integer,
        ADD COLUMN amd_clock_core_state_default              integer,
        ADD COLUMN amd_clock_core_state_is_writable          boolean,
    
        ADD COLUMN amd_clock_memory_lock_minimal             integer,
        ADD COLUMN amd_clock_memory_lock_maximal             integer,
        ADD COLUMN amd_clock_memory_lock_default             integer,
        ADD COLUMN amd_clock_memory_lock_is_writable         boolean,
        ADD COLUMN amd_clock_memory_state_minimal            integer,
        ADD COLUMN amd_clock_memory_state_maximal            integer,
        ADD COLUMN amd_clock_memory_state_default            integer,
        ADD COLUMN amd_clock_memory_state_is_writable        boolean,

        ADD COLUMN amd_voltage_core_lock_minimal             integer,
        ADD COLUMN amd_voltage_core_lock_maximal             integer,
        ADD COLUMN amd_voltage_core_lock_default             integer,
        ADD COLUMN amd_voltage_core_lock_is_writable         boolean,
        ADD COLUMN amd_voltage_core_offset_minimal           integer,
        ADD COLUMN amd_voltage_core_offset_maximal           integer,
        ADD COLUMN amd_voltage_core_offset_default           integer,
        ADD COLUMN amd_voltage_core_offset_is_writable       boolean,
    
        ADD COLUMN amd_voltage_memory_lock_minimal           integer,
        ADD COLUMN amd_voltage_memory_lock_maximal           integer,
        ADD COLUMN amd_voltage_memory_lock_default           integer,
        ADD COLUMN amd_voltage_memory_lock_is_writable       boolean,
        ADD COLUMN amd_voltage_memory_controller_minimal     integer,
        ADD COLUMN amd_voltage_memory_controller_maximal     integer,
        ADD COLUMN amd_voltage_memory_controller_default     integer,
        ADD COLUMN amd_voltage_memory_controller_is_writable boolean,

        ADD COLUMN amd_soc_frequency_minimal                 integer,
        ADD COLUMN amd_soc_frequency_maximal                 integer,
        ADD COLUMN amd_soc_frequency_default                 integer,
        ADD COLUMN amd_soc_frequency_is_writable             boolean,

        ADD COLUMN amd_soc_voltage_minimal                   integer,
        ADD COLUMN amd_soc_voltage_maximal                   integer,
        ADD COLUMN amd_soc_voltage_default                   integer,
        ADD COLUMN amd_soc_voltage_is_writable               boolean,


        -- Добавить параметры amd-разгона в таблицу gpu --

        ADD COLUMN amd_core_clock_lock                       integer,
        ADD COLUMN amd_core_clock_state                      integer,
        ADD COLUMN amd_core_voltage                          integer,
        ADD COLUMN amd_core_voltage_offset                   integer,
        ADD COLUMN amd_memory_clock_lock                     integer,
        ADD COLUMN amd_memory_clock_state                    integer,
        ADD COLUMN amd_memory_voltage                        integer,
        ADD COLUMN amd_memory_controller_voltage             integer,
        ADD COLUMN amd_memory_tweak                          text,
        ADD COLUMN amd_enhanced_overclock                    boolean,
        ADD COLUMN amd_alternative_down_voltage              boolean,
        ADD COLUMN amd_soc_frequency                         integer,
        ADD COLUMN amd_soc_voltage                           integer;


    -- Переименование колокнок с информацией таблицы monitoring_center.gpu --

    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_manufacturer                       TO manufacturer;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_model                              TO model;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_serial_number                      TO serial_number;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_vendor                             TO vendor;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_bios_version                       TO bios_version;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_technology_type                    TO technology_type;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_technology_version                 TO technology_version;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_memory_total                       TO memory_total;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_memory_type                        TO memory_type;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN information_memory_vendor                      TO memory_vendor;


    -- Переименование колокнок с ограничениями таблицы monitoring_center.gpu --

    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_lock_is_writable       TO nvidia_clock_core_lock_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_lock_maximal           TO nvidia_clock_core_lock_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_lock_minimal           TO nvidia_clock_core_lock_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_lock_default           TO nvidia_clock_core_lock_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_offset_is_writable     TO nvidia_clock_core_offset_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_offset_maximal         TO nvidia_clock_core_offset_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_offset_minimal         TO nvidia_clock_core_offset_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_core_offset_default         TO nvidia_clock_core_offset_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_lock_is_writable     TO nvidia_clock_memory_lock_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_lock_maximal         TO nvidia_clock_memory_lock_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_lock_minimal         TO nvidia_clock_memory_lock_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_lock_default         TO nvidia_clock_memory_lock_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_offset_is_writable   TO nvidia_clock_memory_offset_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_offset_maximal       TO nvidia_clock_memory_offset_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_offset_minimal       TO nvidia_clock_memory_offset_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_clock_memory_offset_default       TO nvidia_clock_memory_offset_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_fan_speed_is_writable             TO fan_speed_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_fan_speed_maximal                 TO fan_speed_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_fan_speed_minimal                 TO fan_speed_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_fan_speed_default                 TO fan_speed_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_power_is_writable                 TO power_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_power_maximal                     TO power_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_power_minimal                     TO power_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_power_default                     TO power_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_core_is_writable      TO temperature_core_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_core_maximal          TO temperature_core_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_core_minimal          TO temperature_core_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_core_default          TO temperature_core_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_memory_is_writable    TO temperature_memory_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_memory_maximal        TO temperature_memory_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_memory_minimal        TO temperature_memory_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_temperature_memory_default        TO temperature_memory_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_lock_is_writable     TO nvidia_voltage_core_lock_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_lock_maximal         TO nvidia_voltage_core_lock_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_lock_minimal         TO nvidia_voltage_core_lock_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_lock_default         TO nvidia_voltage_core_lock_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_offset_is_writable   TO nvidia_voltage_core_offset_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_offset_maximal       TO nvidia_voltage_core_offset_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_offset_minimal       TO nvidia_voltage_core_offset_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_core_offset_default       TO nvidia_voltage_core_offset_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_lock_is_writable   TO nvidia_voltage_memory_lock_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_lock_maximal       TO nvidia_voltage_memory_lock_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_lock_minimal       TO nvidia_voltage_memory_lock_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_lock_default       TO nvidia_voltage_memory_lock_default;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_offset_is_writable TO nvidia_voltage_memory_offset_is_writable;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_offset_maximal     TO nvidia_voltage_memory_offset_maximal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_offset_minimal     TO nvidia_voltage_memory_offset_minimal;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN restrictions_voltage_memory_offset_default     TO nvidia_voltage_memory_offset_default;


    -- Переименование колокнок с разгоном таблицы monitoring_center.gpu --

    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_power_limit                       TO power_limit;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_fan_speed                         TO fan_speed;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_core_clock_lock                   TO nvidia_core_clock_lock;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_core_clock_offset                 TO nvidia_core_clock_offset;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_memory_clock_lock                 TO nvidia_memory_clock_lock;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_memory_clock_offset               TO nvidia_memory_clock_offset;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_core_voltage                      TO nvidia_core_voltage;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_core_voltage_offset               TO nvidia_core_voltage_offset;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_memory_voltage                    TO nvidia_memory_voltage;
    ALTER TABLE monitoring_center.gpu RENAME COLUMN overclocking_memory_voltage_offset             TO nvidia_memory_voltage_offset;


    -- Обновление ограничения типов устройств в таблице разгона --

    ALTER TABLE monitoring_center.overclocking
        DROP CONSTRAINT IF EXISTS overclocking_target_device_type_check;

    UPDATE monitoring_center.overclocking ov
    SET target_device_type = CASE
    	WHEN pr.device_name ILIKE 'Nvidia%' THEN 'NvidiaGPU'
    	WHEN pr.device_name ILIKE 'AMD%' THEN 'AmdGPU'
    	WHEN pr.device_name ILIKE 'Intel%' AND ov.fan_speed IS NOT NULL THEN 'IntelGPU'
    	WHEN pr.device_name ILIKE 'Intel%' THEN 'CPU'
    END
    FROM monitoring_center.presets pr
    WHERE pr.overclocking_id = ov.id;

    DELETE FROM monitoring_center.overclocking ov
    WHERE NOT EXISTS (
    	SELECT 1
    	FROM monitoring_center.presets pr
    	WHERE pr.overclocking_id = ov.id
    );
    
    ALTER TABLE monitoring_center.overclocking
        ADD CONSTRAINT overclocking_target_device_type_check CHECK (
            target_device_type = ANY (ARRAY['CPU'::text, 'NvidiaGPU'::text, 'AmdGPU'::text, 'IntelGPU'::text]));


    -- Добавление новых параметров для Overclocking --

    ALTER TABLE monitoring_center.overclocking
        ADD COLUMN core_clock_state integer,
        ADD COLUMN memory_clock_state integer,
        ADD COLUMN memory_controller_voltage integer,
        ADD COLUMN soc_frequency integer,
        ADD COLUMN soc_voltage integer,
        ADD COLUMN memory_tweak text,
        ADD COLUMN enhanced_overclock boolean,
        ADD COLUMN alternative_down_voltage boolean;


    -- Унификация колонки user_id к owner_id в таблицах сущностей мониторинга --

    ALTER TABLE monitoring_center.algorithms
    RENAME CONSTRAINT unique_name_user_id TO unique_name_owner_id;
    
    ALTER INDEX monitoring_center.ix_algorithms_name RENAME TO ix_algorithms_owner_id_name;

    ALTER TABLE monitoring_center.cryptocurrencies
    RENAME COLUMN user_id TO owner_id;

    ALTER INDEX monitoring_center.ix_cryptocurrencies_user_id RENAME TO ix_cryptocurrencies_owner_id;

    ALTER TABLE monitoring_center.flight_sheets
    RENAME COLUMN user_id TO owner_id;

    ALTER INDEX monitoring_center.ix_flight_sheets_user_id RENAME TO ix_flight_sheets_owner_id;

    ALTER INDEX monitoring_center.unique_name_version_user_id RENAME TO unique_name_version_owner_id;

    ALTER TABLE monitoring_center.presets
    RENAME COLUMN user_id TO owner_id;

    ALTER INDEX monitoring_center.ix_presets_user_id RENAME TO ix_presets_owner_id;

    AlTER TABLE monitoring_center.pools
    RENAME COLUMN user_id TO owner_id;

    ALTER INDEX monitoring_center.ix_pools_user_id RENAME TO ix_pools_owner_id;

    ALTER TABLE monitoring_center.wallets
    RENAME COLUMN user_id TO owner_id;

    ALTER INDEX monitoring_center.ix_wallets_user_id RENAME TO ix_wallets_owner_id;


    -- Инстанцирование представлений таблицы gpu --

    -- gpu_info_view --

    CREATE OR REPLACE VIEW monitoring_center.gpu_info_view AS
    SELECT
    	monitoring_center.get_gpu_driver_version(
    		software_inventory.amd_gpu_driver_version,
            software_inventory.intel_gpu_driver_version,
            software_inventory.nvidia_gpu_driver_version,
            gpu.manufacturer
    	) AS driver_version,
    	rigs.name AS rig_name,
    	gpu.* END
    FROM monitoring_center.gpu
    JOIN monitoring_center.rig_inventory ON gpu.rig_inventory_id = rig_inventory.id
    JOIN monitoring_center.rigs ON rig_inventory.rig_id = rigs.id
    JOIN monitoring_center.software_inventory ON software_inventory.rig_inventory_id = rig_inventory.id;

    -- gpu_restrictions_view --

    CREATE OR REPLACE VIEW monitoring_center.gpu_restrictions_view AS
    SELECT
        id,
        rig_inventory_id,
        manufacturer,
        model,
        power_minimal,
        power_maximal,
        power_default,
        power_is_writable,
        fan_speed_minimal,
        fan_speed_maximal,
        fan_speed_default,
        fan_speed_is_writable,
        temperature_core_minimal,
        temperature_core_maximal,
        temperature_core_default,
        temperature_core_is_writable,
        temperature_memory_minimal,
        temperature_memory_maximal,
        temperature_memory_default,
        temperature_memory_is_writable,
        nvidia_clock_core_lock_minimal,
        nvidia_clock_core_lock_maximal,
        nvidia_clock_core_lock_default,
        nvidia_clock_core_lock_is_writable,
        nvidia_clock_core_offset_minimal,
        nvidia_clock_core_offset_maximal,
        nvidia_clock_core_offset_default,
        nvidia_clock_core_offset_is_writable,
        nvidia_clock_memory_lock_minimal,
        nvidia_clock_memory_lock_maximal,
        nvidia_clock_memory_lock_default,
        nvidia_clock_memory_lock_is_writable,
        nvidia_clock_memory_offset_minimal,
        nvidia_clock_memory_offset_maximal,
        nvidia_clock_memory_offset_default,
        nvidia_clock_memory_offset_is_writable,
        nvidia_voltage_core_lock_minimal,
        nvidia_voltage_core_lock_maximal,
        nvidia_voltage_core_lock_default,
        nvidia_voltage_core_lock_is_writable,
        nvidia_voltage_core_offset_minimal,
        nvidia_voltage_core_offset_maximal,
        nvidia_voltage_core_offset_default,
        nvidia_voltage_core_offset_is_writable,
        nvidia_voltage_memory_lock_minimal,
        nvidia_voltage_memory_lock_maximal,
        nvidia_voltage_memory_lock_default,
        nvidia_voltage_memory_lock_is_writable,
        nvidia_voltage_memory_offset_minimal,
        nvidia_voltage_memory_offset_maximal,
        nvidia_voltage_memory_offset_default,
        nvidia_voltage_memory_offset_is_writable,
        amd_clock_core_lock_minimal,
        amd_clock_core_lock_maximal,
        amd_clock_core_lock_default,
        amd_clock_core_lock_is_writable,
        amd_clock_core_state_minimal,
        amd_clock_core_state_maximal,
        amd_clock_core_state_default,
        amd_clock_core_state_is_writable,
        amd_clock_memory_lock_minimal,
        amd_clock_memory_lock_maximal,
        amd_clock_memory_lock_default,
        amd_clock_memory_lock_is_writable,
        amd_clock_memory_state_minimal,
        amd_clock_memory_state_maximal,
        amd_clock_memory_state_default,
        amd_clock_memory_state_is_writable,
        amd_voltage_core_lock_minimal,
        amd_voltage_core_lock_maximal,
        amd_voltage_core_lock_default,
        amd_voltage_core_lock_is_writable,
        amd_voltage_core_offset_minimal,
        amd_voltage_core_offset_maximal,
        amd_voltage_core_offset_default,
        amd_voltage_core_offset_is_writable,
        amd_voltage_memory_lock_minimal,
        amd_voltage_memory_lock_maximal,
        amd_voltage_memory_lock_default,
        amd_voltage_memory_lock_is_writable,
        amd_voltage_memory_controller_minimal,
        amd_voltage_memory_controller_maximal,
        amd_voltage_memory_controller_default,
        amd_voltage_memory_controller_is_writable,
        amd_soc_frequency_minimal,
        amd_soc_frequency_maximal,
        amd_soc_frequency_default,
        amd_soc_frequency_is_writable,
        amd_soc_voltage_minimal,
        amd_soc_voltage_maximal,
        amd_soc_voltage_default,
        amd_soc_voltage_is_writable
    FROM monitoring_center.gpu;


    -- Изменение версии бд --

    UPDATE monitoring_center.version_info SET version = new_version;

END
$$ LANGUAGE plpgsql;