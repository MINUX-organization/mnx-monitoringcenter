CREATE TABLE monitoring_center.miners_inventory
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name text NOT NULL,
    version text NOT NULL,
    rig_inventory_id bigint NOT NULL,

    CONSTRAINT pk_miners_inventory PRIMARY KEY (id),

    CONSTRAINT fk_miners_inventory_software_inventory_rig_inventory_id FOREIGN KEY (rig_inventory_id)
        REFERENCES monitoring_center.software_inventory (rig_inventory_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_miners_inventory_name
    ON monitoring_center.miners_inventory USING btree
    (id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.miners_inventory IS 'майнеры рига';