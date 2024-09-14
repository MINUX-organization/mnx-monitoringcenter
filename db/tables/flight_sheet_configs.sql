CREATE TABLE monitoring_center.flight_sheet_configs
(
    id uuid NOT NULL,
    pool_id uuid NOT NULL,
    pool_password text,
    wallet_id uuid NOT NULL,
    flight_sheet_base_id uuid,

    CONSTRAINT pk_flight_sheet_configs PRIMARY KEY (id),

    CONSTRAINT fk_flight_sheet_configs_flight_sheets_flight_sheet_base_id FOREIGN KEY (flight_sheet_base_id)
        REFERENCES monitoring_center.flight_sheets (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT fk_flight_sheet_configs_pools_pool_id FOREIGN KEY (pool_id)
        REFERENCES monitoring_center.pools (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,

    CONSTRAINT fk_flight_sheet_configs_wallets_wallet_id FOREIGN KEY (wallet_id)
        REFERENCES monitoring_center.wallets (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_flight_sheet_configs_flight_sheet_base_id
    ON monitoring_center.flight_sheet_configs USING btree
    (flight_sheet_base_id ASC NULLS LAST);

CREATE INDEX ix_flight_sheet_configs_pool_id
    ON monitoring_center.flight_sheet_configs USING btree
    (pool_id ASC NULLS LAST);

CREATE INDEX ix_flight_sheet_configs_wallet_id
    ON monitoring_center.flight_sheet_configs USING btree
    (wallet_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.flight_sheet_configs IS 'Конфиги для полётных листов';
