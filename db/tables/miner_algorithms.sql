CREATE TABLE monitoring_center.miner_algorithms
(
    algorithm_id uuid NOT NULL,
    miner_id uuid NOT NULL,
    name text NOT NULL,

    CONSTRAINT pk_miner_algorithms PRIMARY KEY (miner_id, algorithm_id),

    CONSTRAINT fk_miner_algorithms_miners_miner_id FOREIGN KEY (miner_id)
        REFERENCES monitoring_center.miners (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,

    CONSTRAINT fk_miner_algorithms_algorithms_algorithm_id FOREIGN KEY (algorithm_id)
        REFERENCES monitoring_center.algorithms (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

COMMENT ON TABLE monitoring_center.miners IS 'Алгоритмы майнеров';
