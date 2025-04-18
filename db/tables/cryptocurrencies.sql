CREATE TABLE monitoring_center.cryptocurrencies
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    short_name text NOT NULL,
    full_name text NOT NULL,
    algorithm_id uuid NOT NULL,
    user_id uuid NULL,

    CONSTRAINT pk_cryptocurrencies PRIMARY KEY (id),

    CONSTRAINT fk_cryptocurrencies_algorithms_algorithm_id FOREIGN KEY (algorithm_id)
        REFERENCES monitoring_center.algorithms (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_cryptocurrencies_algorithm_id
    ON monitoring_center.cryptocurrencies USING btree
    (algorithm_id ASC NULLS LAST);

CREATE INDEX ix_cryptocurrencies_user_id
    ON monitoring_center.cryptocurrencies USING btree
    (user_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.cryptocurrencies IS 'Криптовалюта';
