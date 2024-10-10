CREATE TABLE monitoring_center.miners
(
    id uuid NOT NULL,
    name text NOT NULL,
    version text NOT NULL,
    device_type monitoring_center.supported_device NOT NULL,
    mining_mode monitoring_center.mining_mode,

    CONSTRAINT pk_miners PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_miners_name
    ON monitoring_center.miners USING btree
    (name ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.miners IS 'Майнеры';
