CREATE TABLE monitoring_center.miners
(
    name text NOT NULL,

    CONSTRAINT pk_miners PRIMARY KEY (name)                        
);

COMMENT ON TABLE monitoring_center.miners IS 'Майнеры';
