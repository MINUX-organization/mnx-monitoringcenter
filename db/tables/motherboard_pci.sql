CREATE TABLE monitoring_center.motherboard_pci
(
    id integer NOT NULL,
    motherboard_id uuid NOT NULL,
    is_installed boolean NOT NULL,
    bus text NOT NULL,

    CONSTRAINT pk_motherboard_pci PRIMARY KEY (id, motherboard_id),

    CONSTRAINT fk_motherboard_pci_motherboard_motherboard_id FOREIGN KEY (motherboard_id)
        REFERENCES monitoring_center.motherboard (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_motherboard_pci_motherboard_id
    ON monitoring_center.motherboard_pci USING btree
    (motherboard_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.motherboard_pci IS 'PCI материнских плат';
