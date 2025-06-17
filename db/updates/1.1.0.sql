\set ON_ERROR_STOP 1
\set ECHO all
\encoding UTF8

DO $$
DECLARE
    current_version TEXT;
    expected_version TEXT := '1.0.0';
    new_version TEXT := '1.1.0';

BEGIN
    
    SELECT version INTO current_version FROM monitoring_center.version_info;

    IF current_version != expected_version THEN
        RAISE EXCEPTION 'The database version % does not match the expected version %', current_version, expected_version;
    END IF;


    -- Создать таблицу miners_inventory --

    CREATE TABLE monitoring_center.miners_inventory
    (
        id uuid NOT NULL DEFAULT uuid_generate_v4(),
        name text NOT NULL,
        version text NOT NULL,
        rig_inventory_id bigint NOT NULL,

        CONSTRAINT pk_miners_inventory PRIMARY KEY (id),

        CONSTRAINT fk_miners_inventory_software_inventory_rig_inventory_id FOREIGN KEY (rig_inventory_id)
            REFERENCES monitoring_center.software_inventory (rig_inventory_id) MATCH SIMPLE
            ON UPDATE NO ACTION
            ON DELETE CASCADE
    );

    CREATE INDEX ix_miners_inventory_name
        ON monitoring_center.miners_inventory USING btree
        (id ASC NULLS LAST);

    COMMENT ON TABLE monitoring_center.miners_inventory IS 'майнеры рига';


    -- Изменить условие первичного ключа software_inventory --

    ALTER TABLE monitoring_center.software_inventory
    DROP CONSTRAINT IF EXISTS pk_software_inventory,
    ADD CONSTRAINT pk_software_inventory PRIMARY KEY (rig_inventory_id);


    -- Перенести все майнеры в таблицу с инвентаризацией манеров --

    INSERT INTO monitoring_center.miners_inventory (name, version, rig_inventory_id)
    SELECT
        key AS name,
        value AS version,
        software_inventory.rig_inventory_id AS rig_inventory_id
    FROM monitoring_center.software_inventory,
    LATERAL json_each_text(monitoring_center.software_inventory.miners::json) AS j(key, value);

    -- Удаление параметров id и miners можно произвести после тестирования нового функционала --


    -- Актуализация таблицы miners --

    ALTER TABLE monitoring_center.miners
    ADD COLUMN owner_id uuid DEFAULT NULL,
    ADD COLUMN type text
        CHECK (type in ('Integrated', 'Custom') ) NOT NULL DEFAULT 'Custom',

    -- Добавление таблицы без ограничения, но после внесения записей необходимо добавить ограничение NOT NULL --
    ADD COLUMN installation_url text, 

    ADD COLUMN pool_template text,
    ADD COLUMN wallet_worker_template text,
    ADD CONSTRAINT unique_name_version_user_id UNIQUE (name, version, owner_id);

    DROP INDEX monitoring_center.ix_miners_name;

    CREATE INDEX ix_miners_name ON monitoring_center.miners USING btree (name ASC NULLS LAST);

    -- Внесение новых записей и актуализация старых записей --
    INSERT INTO monitoring_center.miners (name, version, mining_mode, supported_devices, type, installation_url) values

    ('wildrig-multi', '0.43.0', 'Single', 1+2+8, 'Integrated', 'https://github.com/andru-kun/wildrig-multi/releases/download/0.43.0/wildrig-multi-linux-0.43.0.tar.xz'),
    ('wildrig-multi', '0.42.9', 'Single', 1+2+8, 'Integrated', 'https://github.com/andru-kun/wildrig-multi/releases/download/0.42.9/wildrig-multi-linux-0.42.9.tar.xz'),

    ('t-rex', '0.26.6', 'Dual', 1, 'Integrated', 'https://github.com/trexminer/T-Rex/releases/download/0.26.6/t-rex-0.26.6-linux.tar.gz'),
    ('t-rex', '0.26.5', 'Dual', 1, 'Integrated', 'https://github.com/trexminer/T-Rex/releases/download/0.26.5/t-rex-0.26.5-linux.tar.gz'),

    ('teamredminer', '0.10.20', 'Triple', 8, 'Integrated', 'https://github.com/todxx/teamredminer/releases/download/v0.10.20/teamredminer-v0.10.20-linux.tgz'),

    ('SRBMiner-MULTI', '2.9.0', 'Triple', 1+2+4+8+16, 'Integrated', 'https://github.com/doktor83/SRBMiner-Multi/releases/download/2.9.0/SRBMiner-Multi-2-9-0-Linux.tar.gz'),
    ('SRBMiner-MULTI', '2.8.8', 'Triple', 1+2+4+8+16, 'Integrated', 'https://github.com/doktor83/SRBMiner-Multi/releases/download/2.8.8/SRBMiner-Multi-2-8-8-Linux.tar.gz'),
    ('SRBMiner-MULTI', '2.8.7', 'Triple', 1+2+4+8+16, 'Integrated', 'https://github.com/doktor83/SRBMiner-Multi/releases/download/2.8.7/SRBMiner-Multi-2-8-7-Linux.tar.gz'),

    ('rigel', '1.22.1', 'Triple', 1, 'Integrated', 'https://github.com/rigelminer/rigel/releases/download/1.22.1/rigel-1.22.1-linux.tar.gz'),
    ('rigel', '1.22.0', 'Triple', 1, 'Integrated', 'https://github.com/rigelminer/rigel/releases/download/1.22.0/rigel-1.22.0-linux.tar.gz'),
    ('rigel', '1.21.3', 'Triple', 1, 'Integrated', 'https://github.com/rigelminer/rigel/releases/download/1.21.3/rigel-1.21.3-linux.tar.gz'),
    ('rigel', '1.21.2', 'Triple', 1, 'Integrated', 'https://github.com/rigelminer/rigel/releases/download/1.21.2/rigel-1.21.2-linux.tar.gz'),

    ('onezerominer', '1.4.4', 'Dual', 1, 'Integrated', 'https://github.com/OneZeroMiner/onezerominer/releases/download/v1.4.4/onezerominer-linux-1.4.4.tar.gz'),

    ('lolMiner', '1.95', 'Dual', 1+2+8, 'Integrated', 'https://github.com/Lolliedieb/lolMiner-releases/releases/download/1.95/lolMiner_v1.95_Lin64.tar.gz'),
    ('lolMiner', '1.95a', 'Dual', 1+2+8, 'Integrated', 'https://github.com/Lolliedieb/lolMiner-releases/releases/download/1.95a/lolMiner_v1.95a_Lin64.tar.gz'),

    ('gminer', '3.43', 'Single', 1+8, 'Integrated', 'https://github.com/develsoftware/GMinerRelease/releases/download/3.43/gminer_3_43_linux64.tar.xz'),
    ('gminer', '3.42', 'Single', 1+8, 'Integrated', 'https://github.com/develsoftware/GMinerRelease/releases/download/3.42/gminer_3_42_linux64.tar.xz');

    UPDATE monitoring_center.miners SET type = 'Integrated' WHERE owner_id = null;
    UPDATE monitoring_center.miners
        SET installation_url = CASE
            WHEN name = 'SRBMiner-MULTI' and version = '2.7.6' THEN 'https://github.com/doktor83/SRBMiner-Multi/releases/download/2.7.6/SRBMiner-Multi-2-7-6-Linux.tar.gz'
            WHEN name = 'onezerominer' and version = '1.4.3' THEN 'https://github.com/OneZeroMiner/onezerominer/releases/download/v1.4.3/onezerominer-linux-1.4.3.tar.gz'
            WHEN name = 'gminer' and version = '3.44' THEN 'https://github.com/develsoftware/GMinerRelease/releases/download/3.44/gminer_3_44_linux64.tar.xz'
            WHEN name = 'xmrig' and version = '6.22.2' THEN 'https://github.com/xmrig/xmrig/releases/download/v6.22.2/xmrig-6.22.2-linux-static-x64.tar.gz'
            WHEN name = 'rigel' and version = '1.20.1' THEN 'https://github.com/rigelminer/rigel/releases/download/1.20.1/rigel-1.20.1-linux.tar.gz'
            WHEN name = 'teamredminer' and version = '0.10.21' THEN 'https://github.com/todxx/teamredminer/releases/download/v0.10.21/teamredminer-v0.10.21-linux.tgz'
            WHEN name = 'wildrig-multi' and version = '0.42.2' THEN 'https://github.com/andru-kun/wildrig-multi/releases/download/0.42.2/wildrig-multi-linux-0.42.2.tar.xz'
            WHEN name = 'deroluna-miner' and version = '1.14' THEN 'https://github.com/DeroLuna/dero-miner/releases/download/v1.14/deroluna-v1.14_linux_hiveos_mmpos.tar.gz'
            WHEN name = 'lolMiner' and version = '1.94a' THEN 'https://github.com/Lolliedieb/lolMiner-releases/releases/download/1.94a/lolMiner_v1.94a_Lin64.tar.gz'
            WHEN name = 't-rex' and version = '0.26.8' THEN 'https://github.com/trexminer/T-Rex/releases/download/0.26.8/t-rex-0.26.8-linux.tar.gz'
            ELSE installation_url
        END;


    -- Установка условия NOT NULL для monitoring_center.miners.installation_url --

    ALTER TABLE monitoring_center.miners ALTER COLUMN installation_url SET NOT NULL;


    -- Переименование колонки user_id в owner_id в таблице monitoring_center.algorithms --

    ALTER TABLE monitoring_center.algorithms RENAME COLUMN user_id TO owner_id;


    -- Актуализация таблицы rigs --

    ALTER TABLE monitoring_center.rigs
    ADD COLUMN current_inventory_id bigint,
    ADD COLUMN is_online boolean NOT NULL DEFAULT FALSE,
    ADD COLUMN life_cycle_status text NOT NULL CHECK ( life_cycle_status in ('Disable', 'AwaitsEnable', 'Enable', 'AwaitsDisable') ) DEFAULT 'Disable',
    ADD COLUMN mining_life_cycle_status text NOT NULL CHECK ( mining_life_cycle_status in ('Disable', 'AwaitsEnable', 'Enable', 'AwaitsDisable') ) DEFAULT 'Disable';


    -- Миграция данных в новые столбцы таблицы rigs --
    -- current_inventory_id и life_cycle_status --

    UPDATE monitoring_center.rigs
    SET 
        current_inventory_id = sub.id,
        life_cycle_status = CASE
            WHEN sub.end_date_time IS NULL THEN 'Enable'
            ELSE monitoring_center.rigs.life_cycle_status
        END
    FROM (
        SELECT DISTINCT ON (monitoring_center.rig_inventory.rig_id) 
            monitoring_center.rig_inventory.rig_id,
            monitoring_center.rig_inventory.id,
            monitoring_center.rig_inventory.end_date_time
        FROM monitoring_center.rig_inventory
        WHERE monitoring_center.rig_inventory.is_current = true
        ORDER BY monitoring_center.rig_inventory.rig_id, monitoring_center.rig_inventory.id DESC
    ) sub
    WHERE monitoring_center.rigs.id = sub.rig_id;

    -- is_online --
    UPDATE monitoring_center.rigs
    SET is_online = true
    WHERE EXISTS (
        SELECT 1
        FROM monitoring_center.mining_devices
        WHERE monitoring_center.mining_devices.rig_id = monitoring_center.rigs.id
            AND monitoring_center.mining_devices.life_cycle_status = 'Online'
    );


    -- Изменение версии бд --

    UPDATE monitoring_center.version_info SET version = new_version;

END
$$ LANGUAGE plpgsql;