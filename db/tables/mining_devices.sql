CREATE TABLE monitoring_center.mining_devices
(
    id uuid NOT NULL,
    rig_id uuid NOT NULL,
    owner_id uuid NOT NULL,
    type text NOT NULL CHECK ( type in ('CPU', 'GPU') ),
    is_active boolean NOT NULL,
    flight_sheet_id uuid,

    CONSTRAINT pk_mining_devices PRIMARY KEY (id),

    CONSTRAINT fk_mining_devices_flight_sheets_flight_sheet_id FOREIGN KEY (flight_sheet_id)
        REFERENCES monitoring_center.flight_sheets (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_mining_devices_flight_sheet_id
    ON monitoring_center.mining_devices USING btree
    (flight_sheet_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.mining_devices IS 'Майнинг устройства';
