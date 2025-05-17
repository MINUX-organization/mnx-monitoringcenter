CREATE TABLE monitoring_center.miners
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name text NOT NULL,
    version text NOT NULL,
    type text CHECK ( type in ('Integrated', 'Custom') ) NOT NULL DEFAULT 'Custom',
    supported_devices int NOT NULL,
    mining_mode text CHECK ( mining_mode in ('Single', 'Dual', 'Triple') ) NOT NULL DEFAULT 'Single',
    owner_id uuid,
    installation_url text,
    pool_template text,
    wallet_worker_template text,

    CONSTRAINT pk_miners PRIMARY KEY (id)
);

CREATE INDEX ix_miners_name
    ON monitoring_center.miners USING btree
    (name ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.miners IS 'Майнеры';
