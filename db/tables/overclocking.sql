CREATE TABLE monitoring_center.overclocking
(
    id uuid NOT NULL,
    target_device_type text NOT NULL check( target_device_type in ('CPU', 'NvidiaGPU', 'AmdGPU', 'IntelGPU') ),

    core_clock_lock integer,
    core_clock_offset integer,
    memory_clock_lock integer,
    memory_clock_offset integer,
    core_voltage integer,
    core_voltage_offset integer,
    memory_voltage integer,
    memory_voltage_offset integer,
    power_limit integer,
    core_clock_state integer,
    memory_clock_state integer,
    memory_controller_voltage integer,
    soc_frequency integer,
    soc_voltage integer,
    memory_tweak text,
    enhanced_overclock boolean,
    alternative_down_voltage boolean,
    fan_overclocking_id uuid,

    CONSTRAINT pk_overclocking PRIMARY KEY (id),

    CONSTRAINT fk_overclocking_fan_overclocking_fan_overclocking_id FOREIGN KEY (fan_overclocking_id)
    REFERENCES monitoring_center.fan_overclocking (id) MATCH SIMPLE
    	ON DELETE CASCADE
    	ON UPDATE CASCADE
);

COMMENT ON TABLE monitoring_center.overclocking IS 'Разгон';
