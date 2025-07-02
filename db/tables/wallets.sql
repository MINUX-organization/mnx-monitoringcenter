CREATE TABLE monitoring_center.wallets
(
    id uuid NOT NULL,
    name text NOT NULL,
    address text NOT NULL,
    cryptocurrency_id uuid NOT NULL,
    owner_id uuid NOT NULL,

    CONSTRAINT pk_wallets PRIMARY KEY (id),

    CONSTRAINT fk_wallets_cryptocurrencies_cryptocurrency_id FOREIGN KEY (cryptocurrency_id)
        REFERENCES monitoring_center.cryptocurrencies (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_wallets_cryptocurrency_id
    ON monitoring_center.wallets USING btree
    (cryptocurrency_id ASC NULLS LAST);

CREATE INDEX ix_wallets_owner_id
    ON monitoring_center.wallets USING btree
    (owner_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.wallets IS 'Кошельки';
