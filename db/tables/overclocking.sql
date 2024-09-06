CREATE TABLE monitoring_center.overclocking
(
    id uuid NOT NULL,
    core_clock_lock integer NOT NULL,
    core_clock_offset integer NOT NULL,
    memory_clock_lock integer NOT NULL,
    memory_clock_offset integer NOT NULL,
    core_voltage integer NOT NULL,
    core_voltage_offset integer NOT NULL,
    memory_voltage integer NOT NULL,
    memory_voltage_offset integer NOT NULL,
    power_limit integer NOT NULL,
    critical_temperature integer NOT NULL,
    fan_speed integer NOT NULL,

    CONSTRAINT pk_overclocking PRIMARY KEY (id)
);

COMMENT ON TABLE monitoring_center.overclocking IS 'Разгон';
