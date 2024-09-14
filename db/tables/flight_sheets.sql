CREATE TABLE monitoring_center.flight_sheets
(
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    type integer NOT NULL,
    name text NOT NULL,
    additional_arguments text,
    miner_id uuid NOT NULL,
    huge_page integer,
    config_file text,

    CONSTRAINT pk_flight_sheets PRIMARY KEY (id),

    CONSTRAINT fk_flight_sheets_miners_miner_id FOREIGN KEY (miner_id)
        REFERENCES monitoring_center.miners (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_flight_sheets_miner_id
    ON monitoring_center.flight_sheets USING btree
    (miner_id ASC NULLS LAST);

CREATE INDEX ix_flight_sheets_user_id
    ON monitoring_center.flight_sheets USING btree
    (user_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.flight_sheets IS 'Полётные листы';
