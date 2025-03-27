CREATE TABLE monitoring_center.algorithms
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name text NOT NULL,
    user_id uuid NULL,

    CONSTRAINT pk_algorithms PRIMARY KEY (id),

    CONSTRAINT unique_name_user_id UNIQUE (name, user_id)
);

CREATE UNIQUE INDEX ix_algorithms_name
    ON monitoring_center.algorithms USING btree
    (user_id ASC NULLS LAST, name ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.algorithms IS 'Алгоритмы';

COMMENT ON COLUMN monitoring_center.algorithms.user_id IS 'Если имеет значение null, значит алгоритм доменный';