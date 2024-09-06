CREATE TABLE monitoring_center.cryptocurrencies 
(
    id uuid NOT NULL,
    short_name text NOT NULL,
    full_name text NOT NULL,
    algorithm text NOT NULL,
    user_id uuid NOT NULL,

    CONSTRAINT pk_cryptocurrencies PRIMARY KEY (id),

    CONSTRAINT fk_cryptocurrencies_algorithms_algorithm FOREIGN KEY (algorithm)
        REFERENCES monitoring_center.algorithms (name) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE                        
);

CREATE INDEX ix_cryptocurrencies_algorithm
    ON monitoring_center.cryptocurrencies USING btree
    (algorithm ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.cryptocurrencies IS 'Криптовалюта';
