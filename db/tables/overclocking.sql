CREATE TABLE monitoring_center.overclocking
(
    id uuid NOT NULL,
    target_device_type text NOT NULL check( target_device_type in ('CPU', 'GPU') ),

    core_clock_lock integer,
    core_clock_offset integer,
    memory_clock_lock integer,
    memory_clock_offset integer,
    core_voltage integer,
    core_voltage_offset integer,
    memory_voltage integer,
    memory_voltage_offset integer,
    power_limit integer,
    fan_speed integer,

    CONSTRAINT pk_overclocking PRIMARY KEY (id)
);

COMMENT ON TABLE monitoring_center.overclocking IS 'Разгон';
