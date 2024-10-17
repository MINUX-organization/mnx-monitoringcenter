CREATE TABLE monitoring_center.rigs
(
    id uuid NOT NULL,
    owner_id uuid NOT NULL,
    name text NOT NULL,

    CONSTRAINT pk_rigs PRIMARY KEY (id)
);

COMMENT ON TABLE monitoring_center.rigs IS 'Риги';
