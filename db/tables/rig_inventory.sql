CREATE TABLE monitoring_center.rig_inventory
(
    id bigint NOT NULL,
    rig_id uuid NOT NULL,
    created_date_time timestamp with time zone NOT NULL,
    end_date_time timestamp with time zone,

    CONSTRAINT pk_inventory PRIMARY KEY (id)
);

CREATE INDEX ix_inventory_rig_id_created_date_time
    ON monitoring_center.inventory USING btree
    (rig_id ASC NULLS LAST, created_date_time DESC NULLS FIRST);

COMMENT ON TABLE monitoring_center.inventory IS 'Инвентаризация ригов';
