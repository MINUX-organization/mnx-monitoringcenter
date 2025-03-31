CREATE TABLE monitoring_center.pools
(
    id uuid NOT NULL,
    tls boolean NOT NULL DEFAULT False,
    domain text NOT NULL,
    port integer NOT NULL,
    cryptocurrency_id uuid NOT NULL,
    user_id uuid NULL,

    CONSTRAINT pk_pools PRIMARY KEY (id),

    CONSTRAINT fk_pools_cryptocurrencies_cryptocurrency_id FOREIGN KEY (cryptocurrency_id)
        REFERENCES monitoring_center.cryptocurrencies (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_pools_cryptocurrency_id
    ON monitoring_center.pools USING btree
    (cryptocurrency_id ASC NULLS LAST);

CREATE INDEX ix_pools_user_id
    ON monitoring_center.pools USING btree
    (user_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.pools IS 'Пулы';
