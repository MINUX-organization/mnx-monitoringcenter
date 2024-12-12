CREATE TABLE monitoring_center.cpu
(
    id uuid NOT NULL,
    rig_inventory_id bigint,
    information_architecture text NOT NULL,
    information_cores_count integer NOT NULL,
    information_manufacturer text NOT NULL,
    information_model text NOT NULL,
    information_threads_count integer NOT NULL,
    information_cache_l1 integer NOT NULL,
    information_cache_l2 integer NOT NULL,
    information_cache_l3 integer NOT NULL,
    information_cache_l4 integer,
    pci_bus text NOT NULL,
    pci_id integer NOT NULL,
    restrictions_clock_default integer NOT NULL,
    restrictions_clock_is_writable boolean NOT NULL,
    restrictions_clock_maximal integer NOT NULL,
    restrictions_clock_minimal integer NOT NULL,
    restrictions_fan_speed_default integer NOT NULL,
    restrictions_fan_speed_is_writable boolean NOT NULL,
    restrictions_fan_speed_maximal integer NOT NULL,
    restrictions_fan_speed_minimal integer NOT NULL,
    restrictions_power_default integer NOT NULL,
    restrictions_power_is_writable boolean NOT NULL,
    restrictions_power_maximal integer NOT NULL,
    restrictions_power_minimal integer NOT NULL,
    restrictions_temperature_default integer NOT NULL,
    restrictions_temperature_is_writable boolean NOT NULL,
    restrictions_temperature_maximal integer NOT NULL,
    restrictions_temperature_minimal integer NOT NULL,
    overclocking_core_clock_lock integer NOT NULL,
    overclocking_core_voltage integer NOT NULL,

    CONSTRAINT pk_cpu PRIMARY KEY (rig_inventory_id, id),

    CONSTRAINT fk_cpu_rig_inventory_rig_inventory_id FOREIGN KEY (rig_inventory_id)
        REFERENCES monitoring_center.rig_inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_cpu_inventory_id
    ON monitoring_center.cpu USING btree
    (rig_inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.cpu IS 'Инвентаризация процессоров';
