CREATE TABLE monitoring_center.flight_sheets
(
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    type integer NOT NULL,
    name text NOT NULL,
    additional_arguments text,
    miner text NOT NULL,
    huge_page integer,
    config_file text,

    CONSTRAINT pk_flight_sheets PRIMARY KEY (id),

    CONSTRAINT fk_flight_sheets_miners_miner FOREIGN KEY (miner)
        REFERENCES monitoring_center.miners (name) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_flight_sheets_miner
    ON monitoring_center.flight_sheets USING btree
    (miner ASC NULLS LAST);

CREATE INDEX ix_flight_sheets_user_id
    ON public.flight_sheets USING btree
    (user_id ASC NULLS LAST);
