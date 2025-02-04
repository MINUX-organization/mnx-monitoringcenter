CREATE TABLE monitoring_center.algorithms
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name text NOT NULL,

    CONSTRAINT pk_algorithms PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_algorithms_name
    ON monitoring_center.algorithms USING btree
    (name ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.algorithms IS 'Алгоритмы';
