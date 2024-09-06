CREATE TABLE monitoring_center.algorithms
(
    name text NOT NULL,

    CONSTRAINT pk_algorithms PRIMARY KEY (name)                      
);

COMMENT ON TABLE monitoring_center.algorithms IS 'Алгоритмы';
