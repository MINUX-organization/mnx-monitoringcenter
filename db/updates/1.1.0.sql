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

    UPDATE monitoring_center.miners SET type = 'Integrated' WHERE owner_id is null;
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


    -- Создание новых связок майнеров и алгоритмов в таблице mining_algorithms --

    create function monitoring_center.get_miner_id(miner_name text, miner_version text) returns uuid as $$
    declare
        miner_id uuid;
    begin

        select id into miner_id
        from monitoring_center.miners
        where name = miner_name and version = miner_version;

        return miner_id;
    end;
    $$ language plpgsql;

    create function monitoring_center.get_algorithm_id(algorithm_name text) returns uuid as $$
    declare
        algorithm_id uuid;
    begin

        select id into algorithm_id
        from monitoring_center.algorithms
        where name = algorithm_name;

        return algorithm_id;
    end;
    $$ language plpgsql;

    insert into monitoring_center.miner_algorithms(miner_id, algorithm_id, name)

    select monitoring_center.get_miner_id('rigel', '1.22.1'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Abelian'), 'abelian' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsenhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'nexapow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ton'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512256d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil'
    ) f union all

    select monitoring_center.get_miner_id('rigel', '1.22.0'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Abelian'), 'abelian' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsenhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'nexapow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ton'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512256d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil'
    ) f union all

    select monitoring_center.get_miner_id('rigel', '1.21.3'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Abelian'), 'abelian' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsenhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'nexapow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ton'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512256d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil'
    ) f union all

    select monitoring_center.get_miner_id('rigel', '1.21.2'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Abelian'), 'abelian' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsenhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'nexapow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ton'), 'sha256ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512256d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil'
    ) f union all

    select monitoring_center.get_miner_id('gminer', '3.43'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Zhash (Equihash 144,5)'), 'quihash144_5' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash (125,4)'), 'equihash125_4' UNION ALL
        SELECT monitoring_center.get_algorithm_id('BeamHash III'), 'beamhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash (210,9)'), 'equihash210_9' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckooCycle (Cuckoo29)'), 'cuckoo29' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo32'), 'cuckatoo32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Cortex'), 'cortex' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('AstroBWTv3'), 'astrobwtv3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'radiant' UNION ALL
        SELECT monitoring_center.get_algorithm_id('IronFish (Blake3)'), 'ironfish' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsenhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kaspa (KHeavyHash)'), 'kheavyhash'
    ) f UNION ALL

    select monitoring_center.get_miner_id('gminer', '3.42'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Zhash (Equihash 144,5)'), 'quihash144_5' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash (125,4)'), 'equihash125_4' UNION ALL
        SELECT monitoring_center.get_algorithm_id('BeamHash III'), 'beamhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash (210,9)'), 'equihash210_9' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckooCycle (Cuckoo29)'), 'cuckoo29' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo32'), 'cuckatoo32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Cortex'), 'cortex' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('AstroBWTv3'), 'astrobwtv3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'radiant' UNION ALL
        SELECT monitoring_center.get_algorithm_id('IronFish (Blake3)'), 'ironfish' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsenhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kaspa (KHeavyHash)'), 'kheavyhash'
    ) f UNION ALL

    select monitoring_center.get_miner_id('wildrig-multi', '0.43.0'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('BCD') algorithm_id, 'bcd' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('Bitcore'), 'bitcore' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('C11'), 'c11' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Curvehash'), 'curvehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EvrProgPow'), 'evrprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Hex'), 'hex' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Hmq1725'), 'hmq1725' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MegaBtx'), 'megabtx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Meowpow'), 'meowpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Mike'), 'mike' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'nexapow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Phi'), 'phi' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'progpow-sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow-veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowZ'), 'progpowz' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA-256csm'), 'sha256csm' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512256d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Skein2'), 'skein2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Skunkhash'), 'skunkhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Skydoge'), 'skydoge' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Timetravel'), 'timetravel' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Tribus'), 'tribus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('vProgPoW'), 'vprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X11'), 'x11' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X12'), 'x12' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X13'), 'x13' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X14'), 'x14' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X15'), 'x15' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16r'), 'x16r' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16rv2'), 'x16rv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16rt'), 'x16rt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16s'), 'x16s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X17'), 'x17' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X18'), 'x18' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X21s'), 'x21s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('BMW512'), 'bmw512' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X25x'), 'x25x'
    ) f union all

    select monitoring_center.get_miner_id('wildrig-multi', '0.42.9'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('BCD') algorithm_id, 'bcd' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('Bitcore'), 'bitcore' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('C11'), 'c11' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Curvehash'), 'curvehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EvrProgPow'), 'evrprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Hex'), 'hex' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Hmq1725'), 'hmq1725' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MegaBtx'), 'megabtx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Meowpow'), 'meowpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Mike'), 'mike' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'nexapow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Phi'), 'phi' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'progpow-sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow-veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowZ'), 'progpowz' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA-256csm'), 'sha256csm' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512256d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Skein2'), 'skein2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Skunkhash'), 'skunkhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Skydoge'), 'skydoge' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Timetravel'), 'timetravel' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Tribus'), 'tribus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('vProgPoW'), 'vprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X11'), 'x11' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X12'), 'x12' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X13'), 'x13' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X14'), 'x14' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X15'), 'x15' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16r'), 'x16r' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16rv2'), 'x16rv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16rt'), 'x16rt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16s'), 'x16s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X17'), 'x17' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X18'), 'x18' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X21s'), 'x21s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('BMW512'), 'bmw512' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X25x'), 'x25x'
    ) f union all

    select monitoring_center.get_miner_id('t-rex', '0.26.6'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MTP'), 'mtp' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPow'), 'progpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow-veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowZ'), 'progpowz' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Tensority'), 'tensority' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3'
    ) f union all

    select monitoring_center.get_miner_id('t-rex', '0.26.5'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MTP'), 'mtp' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPow'), 'progpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow-veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowZ'), 'progpowz' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Tensority'), 'tensority' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3'
    ) f union all

    select monitoring_center.get_miner_id('teamredminer', '0.10.20'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Abelian'), 'abel' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verthash'), 'verthash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ton'), 'ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kaspa (KHeavyHash)'), 'kas' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrin' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'karlsen' UNION ALL
        SELECT monitoring_center.get_algorithm_id('IronFish (Blake3)'), 'ironfish' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alph' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Lyra2z'), 'lyra2z' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'ton' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Phi2'), 'phi2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Lyra2REv3'), 'lyra2rev3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16r'), 'x16r' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16rv2'), 'x16rv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('X16s'), 'x16s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MTP'), 'mtp' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo31 (GRIN)'), 'cuckatoo31_grin' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaRood29 (GRIN)'), 'cuckarood29_grin' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight V8'), 'cnv8' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight R (CryptonightV4)'), 'cnr' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Haven'), 'cn_haven' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Heavy'), 'cn_heavy' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Turtle'), 'cnv8_trtl' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight UPX2'), 'cn_upx2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Conceal (CCX)'), 'cn_conceal' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'trtl_chukwa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa2'), 'trtl_chukwa2'
    ) f union all

    select monitoring_center.get_miner_id('SRBMiner-MULTI', '2.9.0'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Argon2d-16000'), 'argon2d_16000' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Argon2d-Dyn (Dynamic)'), 'argon2d_dynamic' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'argon2id_chukwa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa2'), 'argon2id_chukwa2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Aurum'), 'aurum' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Decred (Blake3)'), 'blake3_decred' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CpuPower'), 'cpupower' UNION ALL
        select monitoring_center.get_algorithm_id('CryptoNight Conceal (CCX)'), 'cryptonight_ccx' union all
        SELECT monitoring_center.get_algorithm_id('CryptoNight GPU'), 'cryptonight_gpu' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Turtle'), 'cryptonight_turtle' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight UPX'), 'cryptoonight_upx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Curvehash'), 'curvehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EvrProgPow'), 'evrprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Lyra2REv2 (WebChain)'), 'lyra2v2_webchain' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Meowpow'), 'meowpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Mike'), 'mike' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MinotaurX'), 'minotaurx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Panthera'), 'panthera' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'progpow_quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'progpow_sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow_veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomArq'), 'randomarq' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomEpic'), 'randomepic' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomSCASH'), 'randomscash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomSFX'), 'randomsfx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomTuske'), 'randomtuske' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomX'), 'randomx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomYADA'), 'randomyada' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA256DT'), 'sha256dt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verthash'), 'verthash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verushash'), 'verushash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kaspa (KHeavyHash)'), 'kaspa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512_256d_radiant' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2b)'), 'blake2b' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3_alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('IronFish (Blake3)'), 'blake3_ironfish' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Talleo'), 'cryptonight_talleo' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Keccak'), 'keccak' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash (ApepePow)'), 'memehash_apepepow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('PufferFish2BMB'), 'pufferfish2bmb' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomHash2'), 'randomhash2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomNEVO'), 'randomnevo' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomKEVA'), 'randomkeva' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomGRAFT'), 'randomgrft' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA3d'), 'sha3d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ubqhash'), 'ubqhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yescrypt'), 'yescrypt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR8'), 'yescryptr8' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR16'), 'yescryptr16' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR32'), 'yescryptr32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yespower'), 'yespower' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yespower2B'), 'yespower2b' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerSugar'), 'yespowersugar' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerUrx'), 'yespowerurx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerLtncg'), 'yespowerltncg' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerR16'), 'yespowerr16'
    ) f union all

    select monitoring_center.get_miner_id('SRBMiner-MULTI', '2.8.8'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Argon2d-16000'), 'argon2d_16000' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Argon2d-Dyn (Dynamic)'), 'argon2d_dynamic' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'argon2id_chukwa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa2'), 'argon2id_chukwa2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Aurum'), 'aurum' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Decred (Blake3)'), 'blake3_decred' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CpuPower'), 'cpupower' UNION ALL
        select monitoring_center.get_algorithm_id('CryptoNight Conceal (CCX)'), 'cryptonight_ccx' union all
        SELECT monitoring_center.get_algorithm_id('CryptoNight GPU'), 'cryptonight_gpu' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Turtle'), 'cryptonight_turtle' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight UPX'), 'cryptoonight_upx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Curvehash'), 'curvehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EvrProgPow'), 'evrprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Lyra2REv2 (WebChain)'), 'lyra2v2_webchain' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Meowpow'), 'meowpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Mike'), 'mike' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MinotaurX'), 'minotaurx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Panthera'), 'panthera' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'progpow_quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'progpow_sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow_veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomArq'), 'randomarq' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomEpic'), 'randomepic' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomSCASH'), 'randomscash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomSFX'), 'randomsfx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomTuske'), 'randomtuske' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomX'), 'randomx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomYADA'), 'randomyada' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA256DT'), 'sha256dt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verthash'), 'verthash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verushash'), 'verushash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kaspa (KHeavyHash)'), 'kaspa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512_256d_radiant' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2b)'), 'blake2b' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3_alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('IronFish (Blake3)'), 'blake3_ironfish' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Talleo'), 'cryptonight_talleo' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Keccak'), 'keccak' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash (ApepePow)'), 'memehash_apepepow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('PufferFish2BMB'), 'pufferfish2bmb' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomHash2'), 'randomhash2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomNEVO'), 'randomnevo' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomKEVA'), 'randomkeva' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomGRAFT'), 'randomgrft' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA3d'), 'sha3d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ubqhash'), 'ubqhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yescrypt'), 'yescrypt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR8'), 'yescryptr8' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR16'), 'yescryptr16' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR32'), 'yescryptr32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yespower'), 'yespower' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yespower2B'), 'yespower2b' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerSugar'), 'yespowersugar' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerUrx'), 'yespowerurx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerLtncg'), 'yespowerltncg' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerR16'), 'yespowerr16'
    ) f union all

    select monitoring_center.get_miner_id('SRBMiner-MULTI', '2.8.7'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ethash' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'etchash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Argon2d-16000'), 'argon2d_16000' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Argon2d-Dyn (Dynamic)'), 'argon2d_dynamic' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'argon2id_chukwa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Chukwa2'), 'argon2id_chukwa2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Aurum'), 'aurum' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Decred (Blake3)'), 'blake3_decred' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CpuPower'), 'cpupower' UNION ALL
        select monitoring_center.get_algorithm_id('CryptoNight Conceal (CCX)'), 'cryptonight_ccx' union all
        SELECT monitoring_center.get_algorithm_id('CryptoNight GPU'), 'cryptonight_gpu' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Turtle'), 'cryptonight_turtle' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight UPX'), 'cryptoonight_upx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Curvehash'), 'curvehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ethashb3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('EvrProgPow'), 'evrprogpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FiroPow'), 'firopow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'karlsenhashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kawpow'), 'kawpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Lyra2REv2 (WebChain)'), 'lyra2v2_webchain' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Meowpow'), 'meowpow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Mike'), 'mike' UNION ALL
        SELECT monitoring_center.get_algorithm_id('MinotaurX'), 'minotaurx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Panthera'), 'panthera' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Quai (ProgPow)'), 'progpow_quai' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowSERO (SERO)'), 'progpow_sero' UNION ALL
        SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow_veil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomArq'), 'randomarq' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomEpic'), 'randomepic' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomSCASH'), 'randomscash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomSFX'), 'randomsfx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomTuske'), 'randomtuske' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomX'), 'randomx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomYADA'), 'randomyada' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA256DT'), 'sha256dt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verthash'), 'verthash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Verushash'), 'verushash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelishash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Kaspa (KHeavyHash)'), 'kaspa' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'sha512_256d_radiant' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2b)'), 'blake2b' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3_alephium' UNION ALL
        SELECT monitoring_center.get_algorithm_id('IronFish (Blake3)'), 'blake3_ironfish' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CryptoNight Talleo'), 'cryptonight_talleo' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Keccak'), 'keccak' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Memehash (ApepePow)'), 'memehash_apepepow' UNION ALL
        SELECT monitoring_center.get_algorithm_id('PufferFish2BMB'), 'pufferfish2bmb' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomHash2'), 'randomhash2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomNEVO'), 'randomnevo' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomKEVA'), 'randomkeva' UNION ALL
        SELECT monitoring_center.get_algorithm_id('RandomGRAFT'), 'randomgrft' UNION ALL
        SELECT monitoring_center.get_algorithm_id('SHA3d'), 'sha3d' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ubqhash'), 'ubqhash' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yescrypt'), 'yescrypt' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR8'), 'yescryptr8' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR16'), 'yescryptr16' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YescryptR32'), 'yescryptr32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yespower'), 'yespower' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Yespower2B'), 'yespower2b' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerSugar'), 'yespowersugar' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerUrx'), 'yespowerurx' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerLtncg'), 'yespowerltncg' UNION ALL
        SELECT monitoring_center.get_algorithm_id('YespowerR16'), 'yespowerr16'
    ) f union all

    select monitoring_center.get_miner_id('onezerominer', '1.4.4'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('DynexSolve') algorithm_id, 'dynex' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelis' UNION ALL
        SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2'
    ) f union all

    select monitoring_center.get_miner_id('lolMiner', '1.95'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ETHASH' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'ETCHASH' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'ALEPH' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'AUTOLYKOS2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('BeamHash III'), 'BEAM-III' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckooCycle (Cuckoo29)'), 'C29AE' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo31 (GRIN)'), 'C31' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo32'), 'C32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Zhash (Equihash 144,5)'), 'EQUI144_5' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash Zero (192,7)'), 'EQUI192_7' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash (210,9)'), 'EQUI210_9' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ETHASHB3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'FISHHASH' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'GRAM' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'KARLSEN' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'KARLSENV2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'NEXA' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'OCTOPUS' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'PYRIN' UNION ALL
        SELECT monitoring_center.get_algorithm_id('PyrinhashV2'), 'PYRINV2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'RADIANT' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ubqhash'), 'UBQHASH'
    ) f union all

    select monitoring_center.get_miner_id('lolMiner', '1.95a'), f.algorithm_id, f.name from
    (
        SELECT monitoring_center.get_algorithm_id('Ethash (Ethereum)') algorithm_id, 'ETHASH' name UNION ALL
        SELECT monitoring_center.get_algorithm_id('EtcHash'), 'ETCHASH' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'ALEPH' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'AUTOLYKOS2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('BeamHash III'), 'BEAM-III' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckooCycle (Cuckoo29)'), 'C29AE' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo31 (GRIN)'), 'C31' UNION ALL
        SELECT monitoring_center.get_algorithm_id('CuckaToo32'), 'C32' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Zhash (Equihash 144,5)'), 'EQUI144_5' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash Zero (192,7)'), 'EQUI192_7' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Equihash (210,9)'), 'EQUI210_9' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ethash-B3'), 'ETHASHB3' UNION ALL
        SELECT monitoring_center.get_algorithm_id('FishHash'), 'FISHHASH' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Gram'), 'GRAM' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Karlsenhash'), 'KARLSEN' UNION ALL
        SELECT monitoring_center.get_algorithm_id('KarlsenhashV2'), 'KARLSENV2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('NexaPow'), 'NEXA' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Octopus'), 'OCTOPUS' UNION ALL
        SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'PYRIN' UNION ALL
        SELECT monitoring_center.get_algorithm_id('PyrinhashV2'), 'PYRINV2' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Radiant (SHA-512-256D)'), 'RADIANT' UNION ALL
        SELECT monitoring_center.get_algorithm_id('Ubqhash'), 'UBQHASH'
    ) f;

    drop function monitoring_center.get_miner_id(miner_name text, miner_version text);
    drop function monitoring_center.get_algorithm_id(algorithm_name text);


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