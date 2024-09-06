CREATE TABLE monitoring_center.network_adapter
(
    id uuid NOT NULL,
    global_ip text NOT NULL,
    local_ip text NOT NULL,
    inventory_id bigint,
    information_bus_info text NOT NULL,
    information_logical_name text NOT NULL,
    information_mac text NOT NULL,
    information_manufacturer text NOT NULL,
    information_model text NOT NULL,
    information_serial_number text NOT NULL,
    information_vendor_code text NOT NULL,

    CONSTRAINT pk_network_adapter PRIMARY KEY (id),

    CONSTRAINT fk_network_adapter_inventory_inventory_id FOREIGN KEY (inventory_id)
        REFERENCES monitoring_center.inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE INDEX ix_network_adapter_inventory_id
    ON monitoring_center.network_adapter USING btree
    (inventory_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.network_adapter IS 'Инвентаризация сетевых адаптеров';
