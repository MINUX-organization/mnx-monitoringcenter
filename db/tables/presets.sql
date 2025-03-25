CREATE TABLE monitoring_center.presets
(
    id uuid NOT NULL,
    name text NOT NULL,
    device_name text NOT NULL,
    overclocking_id uuid NOT NULL,
    user_id uuid NOT NULL,
    is_visible boolean NOT NULL,

    CONSTRAINT pk_presets PRIMARY KEY (id),

    CONSTRAINT fk_presets_overclocking_overclocking_id FOREIGN KEY (overclocking_id)
        REFERENCES monitoring_center.overclocking (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_presets_overclocking_id
    ON monitoring_center.presets USING btree
    (overclocking_id ASC NULLS LAST);

CREATE INDEX ix_presets_user_id
    ON monitoring_center.presets USING btree
    (user_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.presets IS 'Пресеты';