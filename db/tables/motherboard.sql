CREATE TABLE monitoring_center.motherboard
(
    id uuid NOT NULL,
    rig_inventory_id bigint,
    information_manufacturer text NOT NULL,
    information_model text NOT NULL,
    information_pci_x16posrts_count integer NOT NULL,
    information_pci_x4posrts_count integer NOT NULL,
    information_ram_ports_count integer NOT NULL,
    information_sata_ports_count integer NOT NULL,
    information_serial_number text NOT NULL,

    CONSTRAINT pk_motherboard PRIMARY KEY (id),

    CONSTRAINT fk_motherboard_rig_inventory_inventory_id FOREIGN KEY (rig_inventory_id)
        REFERENCES monitoring_center.rig_inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE UNIQUE INDEX ix_motherboard_inventory_id
    ON monitoring_center.motherboard USING btree
    (rig_inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.motherboard IS 'Инвентаризация материнских плат';
