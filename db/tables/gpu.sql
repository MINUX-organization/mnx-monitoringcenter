CREATE TABLE monitoring_center.gpu
(
    id uuid NOT NULL,
    inventory_id bigint,
    information_bios_version text NOT NULL,
    information_manufacturer text NOT NULL,
    information_model text NOT NULL,
    information_serial_number text NOT NULL,
    information_vendor text NOT NULL,
    information_memory_total integer NOT NULL,
    information_memory_type text NOT NULL,
    information_memory_vendor text NOT NULL,
    information_technology_type integer NOT NULL,
    information_technology_version text NOT NULL,
    pci_bus integer NOT NULL,
    pci_id integer NOT NULL,
    restrictions_clock_core_lock_default integer NOT NULL,
    restrictions_clock_core_lock_is_writable boolean NOT NULL,
    restrictions_clock_core_lock_maximal integer NOT NULL,
    restrictions_clock_core_lock_minimal integer NOT NULL,
    restrictions_clock_core_offset_default integer NOT NULL,
    restrictions_clock_core_offset_is_writable boolean NOT NULL,
    restrictions_clock_core_offset_maximal integer NOT NULL,
    restrictions_clock_core_offset_minimal integer NOT NULL,
    restrictions_clock_memory_lock_default integer NOT NULL,
    restrictions_clock_memory_lock_is_writable boolean NOT NULL,
    restrictions_clock_memory_lock_maximal integer NOT NULL,
    restrictions_clock_memory_lock_minimal integer NOT NULL,
    restrictions_clock_memory_offset_default integer NOT NULL,
    restrictions_clock_memory_offset_is_writable boolean NOT NULL,
    restrictions_clock_memory_offset_maximal integer NOT NULL,
    restrictions_clock_memory_offset_minimal integer NOT NULL,
    restrictions_fan_speed_default integer NOT NULL,
    restrictions_fan_speed_is_writable boolean NOT NULL,
    restrictions_fan_speed_maximal integer NOT NULL,
    restrictions_fan_speed_minimal integer NOT NULL,
    restrictions_power_default integer NOT NULL,
    restrictions_power_is_writable boolean NOT NULL,
    restrictions_power_maximal integer NOT NULL,
    restrictions_power_minimal integer NOT NULL,
    restrictions_temperature_core_default integer NOT NULL,
    restrictions_temperature_core_is_writable boolean NOT NULL,
    restrictions_temperature_core_maximal integer NOT NULL,
    restrictions_temperature_core_minimal integer NOT NULL,
    restrictions_temperature_memory_default integer NOT NULL,
    restrictions_temperature_memory_is_writable boolean NOT NULL,
    restrictions_temperature_memory_maximal integer NOT NULL,
    restrictions_temperature_memory_minimal integer NOT NULL,
    restrictions_voltage_core_lock_default integer NOT NULL,
    restrictions_voltage_core_lock_is_writable boolean NOT NULL,
    restrictions_voltage_core_lock_maximal integer NOT NULL,
    restrictions_voltage_core_lock_minimal integer NOT NULL,
    restrictions_voltage_core_offset_default integer NOT NULL,
    restrictions_voltage_core_offset_is_writable boolean NOT NULL,
    restrictions_voltage_core_offset_maximal integer NOT NULL,
    restrictions_voltage_core_offset_minimal integer NOT NULL,
    restrictions_voltage_memory_lock_default integer NOT NULL,
    restrictions_voltage_memory_lock_is_writable boolean NOT NULL,
    restrictions_voltage_memory_lock_maximal integer NOT NULL,
    restrictions_voltage_memory_lock_minimal integer NOT NULL,
    restrictions_voltage_memory_offset_default integer NOT NULL,
    restrictions_voltage_memory_offset_is_writable boolean NOT NULL,
    restrictions_voltage_memory_offset_maximal integer NOT NULL,
    restrictions_voltage_memory_offset_minimal integer NOT NULL,

    CONSTRAINT pk_gpu PRIMARY KEY (id),

    CONSTRAINT fk_gpu_inventory_inventory_id FOREIGN KEY (inventory_id)
        REFERENCES monitoring_center.inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_gpu_inventory_id
    ON monitoring_center.gpu USING btree
    (inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.gpu IS 'Инвентаризация видеокарт';
