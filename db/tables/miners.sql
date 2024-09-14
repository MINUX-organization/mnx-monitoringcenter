CREATE TABLE monitoring_center.miners
(
    id uuid NOT NULL,
    name text NOT NULL,

    CONSTRAINT pk_miners PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_miners_name
    ON monitoring_center.miners USING btree
    (name ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.miners IS 'Майнеры';
