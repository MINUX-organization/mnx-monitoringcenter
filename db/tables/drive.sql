CREATE TABLE monitoring_center.drive
(
    id uuid NOT NULL,
    rig_inventory_id bigint NOT NULL,
    information_capacity integer,
    information_manufacturer text,
    information_model text,
    information_serial_number text,

    CONSTRAINT pk_drive PRIMARY KEY (rig_inventory_id, id),

    CONSTRAINT fk_drive_rig_inventory_rig_inventory_id FOREIGN KEY (rig_inventory_id)
        REFERENCES monitoring_center.rig_inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_drive_inventory_id
    ON monitoring_center.drive USING btree
    (rig_inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.drive IS 'Инвентаризация дисков';
