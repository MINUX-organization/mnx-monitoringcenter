-- ########################## CREATE FUNCTIONS ##############################

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

-- ########################## INSERT ##############################

insert into monitoring_center.miner_algorithms(miner_id, algorithm_id, name)

select monitoring_center.get_miner_id('deroluna-miner', '1.13'), f.algorithm_id, f.name from
(
    select monitoring_center.get_algorithm_id('AstroBWTv3') algorithm_id, 'astrobwt' name
) f union all

select monitoring_center.get_miner_id('xmrig', '6.22.0'), f.algorithm_id, f.name from
(
    select monitoring_center.get_algorithm_id('GhostRider (Raptoreum)') algorithm_id, 'gr' name union all
    select monitoring_center.get_algorithm_id('RandomKEVA'), 'rx/keva' union all
    select monitoring_center.get_algorithm_id('RandomGRAFT'), 'rx/graft' union all
    select monitoring_center.get_algorithm_id('RandomSFX'), 'rx/sfx' union all
    select monitoring_center.get_algorithm_id('CryptoNightFemto'), 'cn/upx2' union all
    select monitoring_center.get_algorithm_id('Conceal (CXX)'), 'cn/ccx' union all
    select monitoring_center.get_algorithm_id('CryptoNightTalleo'), 'cn-pico/tlo' union all
    select monitoring_center.get_algorithm_id('CryptoNightPico'), 'cn-pico' union all
    select monitoring_center.get_algorithm_id('RandomARQ'), 'rx/arq' union all
    select monitoring_center.get_algorithm_id('RandomX'), 'rx/0' union all
    select monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'argon2/chukwa' union all
    select monitoring_center.get_algorithm_id('NINJA (Argon2id-NINJA)'), 'argon2/ninja' union all
    select monitoring_center.get_algorithm_id('RandomWOW'), 'rx/wow' union all
    select monitoring_center.get_algorithm_id('CryptoNightR'), 'cn/r'
) f;

-- ########################## DELETE FUNCTIONS ##############################

drop function monitoring_center.get_miner_id(miner_name text, miner_version text);
drop function monitoring_center.get_algorithm_id(algorithm_name text);
