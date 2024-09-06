CREATE TABLE monitoring_center.drive
(
    id uuid NOT NULL,
    inventory_id bigint,
    information_capacity integer NOT NULL,
    information_manufacturer text NOT NULL,
    information_model text NOT NULL,
    information_serial_number text NOT NULL,

    CONSTRAINT pk_drive PRIMARY KEY (id),

    CONSTRAINT fk_drive_inventory_inventory_id FOREIGN KEY (inventory_id)
        REFERENCES monitoring_center.inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_drive_inventory_id
    ON monitoring_center.drive USING btree
    (inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.drive IS 'Инвентаризация дисков';
