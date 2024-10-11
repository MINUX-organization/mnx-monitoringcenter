CREATE TABLE monitoring_center.supported_device_type
(
    miner_id uuid NOT NULL,
    device_type monitoring_center.supported_device NOT NULL,

    CONSTRAINT pk_supported_device_type PRIMARY KEY (miner_id, device_type),

    CONSTRAINT fk_supported_device_type_miners_miner_id FOREIGN KEY (miner_id)
        REFERENCES monitoring_center.miners (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_supported_device_type_miner_id
    ON monitoring_center.supported_device_type USING btree
    (miner_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.supported_device_type IS 'Поддерживаемые типы дивайсов';