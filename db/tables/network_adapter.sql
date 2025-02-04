CREATE TABLE monitoring_center.network_adapter
(
    id uuid NOT NULL,
    global_ip text,
    local_ip text,
    rig_inventory_id bigint NOT NULL,
    information_bus_info text,
    information_logical_name text NOT NULL,
    information_mac text NOT NULL,
    information_manufacturer text,
    information_model text,
    information_serial_number text,
    information_vendor_code text,

    CONSTRAINT pk_network_adapter PRIMARY KEY (rig_inventory_id, id),

    CONSTRAINT fk_network_adapter_rig_inventory_rig_inventory_id FOREIGN KEY (rig_inventory_id)
        REFERENCES monitoring_center.rig_inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_network_adapter_inventory_id
    ON monitoring_center.network_adapter USING btree
    (rig_inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.network_adapter IS 'Инвентаризация сетевых адаптеров';
