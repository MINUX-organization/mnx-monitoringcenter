CREATE TABLE monitoring_center.gpu
(
    id uuid NOT NULL,
    rig_inventory_id bigint NOT NULL,

    -- PCI --
    pci_id integer NOT NULL,
    pci_bus text NOT NULL,

    -- Information --
    information_manufacturer       text NOT NULL,
    information_model              text NOT NULL DEFAULT 'Unknown',
    information_serial_number      text,
    information_vendor             text,
    information_bios_version       text,
    information_technology_type    integer NOT NULL,
    information_technology_version text,
    information_memory_total       integer NOT NULL,
    information_memory_type        text,
    information_memory_vendor      text,

    -- Restrictions --
    restrictions_power_minimal     integer NOT NULL,
    restrictions_power_maximal     integer NOT NULL,
    restrictions_power_default     integer,
    restrictions_power_is_writable boolean NOT NULL,

    restrictions_fan_speed_minimal     integer NOT NULL,
    restrictions_fan_speed_maximal     integer NOT NULL,
    restrictions_fan_speed_default     integer,
    restrictions_fan_speed_is_writable boolean NOT NULL,
    
    restrictions_temperature_core_minimal       integer NOT NULL,
    restrictions_temperature_core_maximal       integer NOT NULL,
    restrictions_temperature_core_default       integer,
    restrictions_temperature_core_is_writable   boolean NOT NULL,
    restrictions_temperature_memory_minimal     integer NOT NULL,
    restrictions_temperature_memory_maximal     integer NOT NULL,
    restrictions_temperature_memory_default     integer,
    restrictions_temperature_memory_is_writable boolean NOT NULL,

    restrictions_clock_core_lock_minimal         integer NOT NULL,
    restrictions_clock_core_lock_maximal         integer NOT NULL,
    restrictions_clock_core_lock_default         integer,
    restrictions_clock_core_lock_is_writable     boolean NOT NULL,
    restrictions_clock_core_offset_minimal       integer NOT NULL,
    restrictions_clock_core_offset_maximal       integer NOT NULL,
    restrictions_clock_core_offset_default       integer,
    restrictions_clock_core_offset_is_writable   boolean NOT NULL,
    restrictions_clock_memory_lock_minimal       integer NOT NULL,
    restrictions_clock_memory_lock_maximal       integer NOT NULL,
    restrictions_clock_memory_lock_default       integer,
    restrictions_clock_memory_lock_is_writable   boolean NOT NULL,
    restrictions_clock_memory_offset_minimal     integer NOT NULL,
    restrictions_clock_memory_offset_maximal     integer NOT NULL,
    restrictions_clock_memory_offset_default     integer,
    restrictions_clock_memory_offset_is_writable boolean NOT NULL,
    
    restrictions_voltage_core_lock_minimal         integer NOT NULL,
    restrictions_voltage_core_lock_maximal         integer NOT NULL,
    restrictions_voltage_core_lock_default         integer,
    restrictions_voltage_core_lock_is_writable     boolean NOT NULL,
    restrictions_voltage_core_offset_minimal       integer NOT NULL,
    restrictions_voltage_core_offset_maximal       integer NOT NULL,
    restrictions_voltage_core_offset_default       integer,
    restrictions_voltage_core_offset_is_writable   boolean NOT NULL,
    restrictions_voltage_memory_lock_minimal       integer NOT NULL,
    restrictions_voltage_memory_lock_maximal       integer NOT NULL,
    restrictions_voltage_memory_lock_default       integer,
    restrictions_voltage_memory_lock_is_writable   boolean NOT NULL,
    restrictions_voltage_memory_offset_minimal     integer NOT NULL,
    restrictions_voltage_memory_offset_maximal     integer NOT NULL,
    restrictions_voltage_memory_offset_default     integer,
    restrictions_voltage_memory_offset_is_writable boolean NOT NULL,

    -- Overclocking --

    overclocking_power_limit           integer,
    overclocking_fan_speed             integer,
    overclocking_core_clock_lock       integer,
    overclocking_core_clock_offset     integer,
    overclocking_memory_clock_lock     integer,
    overclocking_memory_clock_offset   integer,
    overclocking_core_voltage          integer,
    overclocking_core_voltage_offset   integer,
    overclocking_memory_voltage        integer,
    overclocking_memory_voltage_offset integer,

    CONSTRAINT pk_gpu PRIMARY KEY (rig_inventory_id, id),

    CONSTRAINT fk_gpu_rig_inventory_rig_inventory_id FOREIGN KEY (rig_inventory_id)
        REFERENCES monitoring_center.rig_inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_gpu_inventory_id
    ON monitoring_center.gpu USING btree
    (rig_inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.gpu IS 'Инвентаризация видеокарт';
