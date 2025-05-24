CREATE TABLE monitoring_center.miners_inventory
(
    id uuid NOT NULL,
    name text NOT NULL,
    version text NOT NULL,
    software_id bigint NOT NULL,
    rig_inventory_id bigint NOT NULL,

    CONSTRAINT pk_miners_inventory PRIMARY KEY (id),

    CONSTRAINT fk_miners_inventory_software_inventory_id_rig_inventory_id FOREIGN KEY (software_id, rig_inventory_id)
        REFERENCES monitoring_center.software_inventory (id, rig_inventory_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_miners_inventory_name
    ON monitoring_center.miners_inventory USING btree
    (id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.miners_inventory IS 'майнеры рига';