CREATE TABLE monitoring_center.rigs
(
    id uuid NOT NULL,
    owner_id uuid NOT NULL,
    name text NOT NULL,
    current_inventory_id bigint,
    is_online boolean NOT NULL DEFAULT FALSE,
    life_cycle_status text NOT NULL
        CHECK ( life_cycle_status in ('Disable', 'AwaitsEnable', 'Enable', 'AwaitsDisable') ) DEFAULT 'Disable',
    mining_life_cycle_status text NOT NULL
        CHECK ( mining_life_cycle_status in ('Disable', 'AwaitsEnable', 'Enable', 'AwaitsDisable') ) DEFAULT 'Disable',

    CONSTRAINT pk_rigs PRIMARY KEY (id)
);

COMMENT ON TABLE monitoring_center.rigs IS 'Риги';
