CREATE TABLE monitoring_center.software_inventory
(
    id uuid NOT NULL,
    minux_version text NOT NULL,
    linux_version text NOT NULL,
    amd_driver_version text NOT NULL,
    nvidia_driver_version text NOT NULL,
    intel_driver_version text NOT NULL,
    open_cl_version text NOT NULL,
    cuda_version text NOT NULL,
    agent_version text NOT NULL,
    hardware_manager_version text NOT NULL,
    miners text NOT NULL,
    inventory_id bigint,

    CONSTRAINT pk_software_inventory PRIMARY KEY (id),

    CONSTRAINT fk_software_inventory_inventory_inventory_id FOREIGN KEY (inventory_id)
        REFERENCES monitoring_center.inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE UNIQUE INDEX IF NOT EXISTS ix_software_inventory_inventory_id
    ON monitoring_center.software_inventory USING btree
    (inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.software_inventory IS 'Инвентаризация программного обеспечения';
