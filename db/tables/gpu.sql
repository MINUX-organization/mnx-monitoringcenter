CREATE TABLE monitoring_center.gpu
(
    id uuid NOT NULL,
    rig_inventory_id bigint NOT NULL,

    -- PCI --
    pci_id integer NOT NULL,
    pci_bus text NOT NULL,

    -- Information --
    manufacturer text NOT NULL,
    model text NOT NULL DEFAULT 'Unknown',
    serial_number text,
    vendor text,
    bios_version text,
    technology_type integer NOT NULL,
    technology_version text,
    memory_total integer NOT NULL,
    memory_type text,
    memory_vendor text,

    -- Restrictions --
    power_minimal integer,
    power_maximal integer,
    power_default integer,
    power_is_writable boolean,

    fan_speed_minimal integer,
    fan_speed_maximal integer,
    fan_speed_default integer,
    fan_speed_is_writable boolean,
    
    temperature_core_minimal integer,
    temperature_core_maximal integer,
    temperature_core_default integer,
    temperature_core_is_writable boolean,
    temperature_memory_minimal integer,
    temperature_memory_maximal integer,
    temperature_memory_default integer,
    temperature_memory_is_writable boolean,

    nvidia_clock_core_lock_minimal integer,
    nvidia_clock_core_lock_maximal integer,
    nvidia_clock_core_lock_default integer,
    nvidia_clock_core_lock_is_writable boolean,
    nvidia_clock_core_offset_minimal integer,
    nvidia_clock_core_offset_maximal integer,
    nvidia_clock_core_offset_default integer,
    nvidia_clock_core_offset_is_writable boolean,
    nvidia_clock_memory_lock_minimal integer,
    nvidia_clock_memory_lock_maximal integer,
    nvidia_clock_memory_lock_default integer,
    nvidia_clock_memory_lock_is_writable boolean,
    nvidia_clock_memory_offset_minimal integer,
    nvidia_clock_memory_offset_maximal integer,
    nvidia_clock_memory_offset_default integer,
    nvidia_clock_memory_offset_is_writable boolean,
    
    nvidia_voltage_core_lock_minimal integer,
    nvidia_voltage_core_lock_maximal integer,
    nvidia_voltage_core_lock_default integer,
    nvidia_voltage_core_lock_is_writable boolean,
    nvidia_voltage_core_offset_minimal integer,
    nvidia_voltage_core_offset_maximal integer,
    nvidia_voltage_core_offset_default integer,
    nvidia_voltage_core_offset_is_writable boolean,
    nvidia_voltage_memory_lock_minimal integer,
    nvidia_voltage_memory_lock_maximal integer,
    nvidia_voltage_memory_lock_default integer,
    nvidia_voltage_memory_lock_is_writable boolean,
    nvidia_voltage_memory_offset_minimal integer,
    nvidia_voltage_memory_offset_maximal integer,
    nvidia_voltage_memory_offset_default integer,
    nvidia_voltage_memory_offset_is_writable boolean,

    amd_clock_core_lock_minimal integer,
	amd_clock_core_lock_maximal integer,
	amd_clock_core_lock_default integer,
	amd_clock_core_lock_is_writable boolean,
	amd_clock_core_state_minimal integer,
	amd_clock_core_state_maximal integer,
	amd_clock_core_state_default integer,
	amd_clock_core_state_is_writable boolean,

	amd_clock_memory_lock_minimal integer,
	amd_clock_memory_lock_maximal integer,
	amd_clock_memory_lock_default integer,
	amd_clock_memory_lock_is_writable boolean,
	amd_clock_memory_state_minimal integer,
	amd_clock_memory_state_maximal integer,
	amd_clock_memory_state_default integer,
	amd_clock_memory_state_is_writable boolean,

	amd_voltage_core_lock_minimal integer,
	amd_voltage_core_lock_maximal integer,
	amd_voltage_core_lock_default integer,
	amd_voltage_core_lock_is_writable boolean,
	amd_voltage_core_offset_minimal integer,
	amd_voltage_core_offset_maximal integer,
	amd_voltage_core_offset_default integer,
	amd_voltage_core_offset_is_writable boolean,

	amd_voltage_memory_lock_minimal integer,
	amd_voltage_memory_lock_maximal integer,
	amd_voltage_memory_lock_default integer,
	amd_voltage_memory_lock_is_writable boolean,
	amd_voltage_memory_controller_minimal integer,
	amd_voltage_memory_controller_maximal integer,
	amd_voltage_memory_controller_default integer,
	amd_voltage_memory_controller_is_writable boolean,

    amd_soc_frequency_minimal integer,
    amd_soc_frequency_maximal integer,
    amd_soc_frequency_default integer,
    amd_soc_frequency_is_writable boolean,
    amd_soc_voltage_minimal integer,
    amd_soc_voltage_maximal integer,
    amd_soc_voltage_default integer,
    amd_soc_voltage_is_writable boolean,

    -- Overclocking --

    power_limit integer,
    fan_speed integer,

    nvidia_core_clock_lock       integer,
    nvidia_core_clock_offset     integer,
    nvidia_memory_clock_lock     integer,
    nvidia_memory_clock_offset   integer,
    nvidia_core_voltage          integer,
    nvidia_core_voltage_offset   integer,
    nvidia_memory_voltage        integer,
    nvidia_memory_voltage_offset integer,

    amd_core_clock_lock integer,
    amd_core_clock_state integer,
    amd_core_voltage integer,
    amd_core_voltage_offset integer,
    amd_memory_clock_lock integer,
    amd_memory_clock_state integer,
    amd_memory_voltage integer,
    amd_memory_controller_voltage integer,
    amd_memory_tweak text,
    amd_enhanced_overclock boolean,
    amd_alternative_down_voltage boolean,
    amd_soc_frequency integer,
    amd_soc_voltage integer,

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
