CREATE TABLE monitoring_center.fan_overclocking
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    fan_overclocking_type text NOT NULL CHECK (
        type in ('TargetSpeed', 'TargetTemperature', 'LinearDependence')),
    target_speed integer,
    min_target_speed integer,
    max_target_speed integer,
    target_core_temperature integer,
    target_memory_temperature integer,
    target_points jsonb,

    CONSTRAINT pk_fan_overclocking PRIMARY KEY (id)
);

COMMENT ON TABLE monitoring_center.fan_overclocking IS 'разгон скорости вентилятора';