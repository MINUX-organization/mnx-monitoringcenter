CREATE TABLE monitoring_center.flight_sheet_targets
(
    id uuid NOT NULL,
    type integer NOT NULL,
    additional_arguments text,
    miner_id uuid NOT NULL,
    flight_sheet_id uuid NOT NULL,
    huge_pages integer,
    config_file_content text,
    threads_count integer

    CONSTRAINT pk_flight_sheet_targets PRIMARY KEY (id),

    CONSTRAINT fk_flight_sheet_targets_flight_sheets_flight_sheet_id FOREIGN KEY (flight_sheet_id)
        REFERENCES monitoring_center.flight_sheets (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,

    CONSTRAINT fk_flight_sheet_targets_miners_miner_id FOREIGN KEY (miner_id)
        REFERENCES monitoring_center.miners (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_flight_sheet_targets_flight_sheet_id
    ON monitoring_center.flight_sheet_targets USING btree
    (flight_sheet_id ASC NULLS LAST);

CREATE INDEX ix_flight_sheet_targets_miner_id
    ON monitoring_center.flight_sheet_targets USING btree
    (miner_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.flight_sheet_targets IS 'Таргеты полётных листов';
