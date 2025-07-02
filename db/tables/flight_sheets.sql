CREATE TABLE monitoring_center.flight_sheets
(
    id uuid NOT NULL,
    name text NOT NULL,
    owner_id uuid NOT NULL,

    CONSTRAINT pk_flight_sheets PRIMARY KEY (id)
);

CREATE INDEX ix_flight_sheets_name
    ON monitoring_center.flight_sheets USING btree
    (name ASC NULLS LAST);

CREATE INDEX ix_flight_sheets_owner_id
    ON monitoring_center.flight_sheets USING btree
    (owner_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.flight_sheets IS 'Полётные листы';
