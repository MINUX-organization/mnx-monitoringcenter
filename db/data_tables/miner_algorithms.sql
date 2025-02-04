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

select monitoring_center.get_miner_id('deroluna-miner', '1.14'), f.algorithm_id, f.name from
(
    select monitoring_center.get_algorithm_id('AstroBWTv3') algorithm_id, 'astrobwt' name
) f union all

select monitoring_center.get_miner_id('xmrig', '6.22.2'), f.algorithm_id, f.name from
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
) f union all

select monitoring_center.get_miner_id('wildrig-multi', '0.42.2'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('BCD') algorithm_id, 'bcd' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3' UNION ALL
    SELECT monitoring_center.get_algorithm_id('BMW512'), 'bmw512' UNION ALL
    SELECT monitoring_center.get_algorithm_id('C11'), 'c11' UNION ALL
    SELECT monitoring_center.get_algorithm_id('FiroPoW'), 'firopow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HMQ1725'), 'hmq1725' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KawPoW'), 'kawpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('NexaPoW'), 'nexapow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowSERO'), 'progpow-sero' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow-veil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowZ'), 'progpowz' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA-256csm'), 'sha256csm' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA-512-256D (Radiant)'), 'sha512256d' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Tribus'), 'tribus' UNION ALL
    SELECT monitoring_center.get_algorithm_id('vProgPoW'), 'vprogpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16R'), 'x16r' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16RT'), 'x16r' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16Rv2'), 'x16rv2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16S'), 'x16s' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X17'), 'x17' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X21S'), 'x21s' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X25X'), 'x25x' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xevan'), 'xevan'
) f union all

select monitoring_center.get_miner_id('T-Rex', '0.26.8'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('Ethash') algorithm_id, 'ethash' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Etchash'), 'etchash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('FiroPoW'), 'firopow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow-veil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowZ'), 'progpowz' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KawPoW'), 'kawpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3'
) f union all

select monitoring_center.get_miner_id('TeamRedMiner', '0.10.21'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('Ethash') algorithm_id, 'ethash' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Etchash'), 'etchash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KawPoW'), 'kawpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alph' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Gram'), 'ton' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (IronFish)'), 'ironfish' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KarlsenHash'), 'karlsen' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Verthash'), 'verthash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KHeavyHash (Kaspa)'), 'kas' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrin' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightHaven'), 'cn_haven' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightR'), 'cnr' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightTurtle'), 'cnv8_trtl' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightUPX2'), 'cn_upx2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightConceal (Conceal)'), 'cn_conceal' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'trtl_chukwa' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ton'), 'ton' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16R'), 'x16r' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16Rv2'), 'x16rv2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('X16S'), 'x16s' UNION ALL
    SELECT monitoring_center.get_algorithm_id('cuckAToo31'), 'cuckatoo31_grin' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Lyra2z'), 'lyra2z'
) f union all

select monitoring_center.get_miner_id('SRBMiner-Multi', '2.7.6'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('Ethash') algorithm_id, 'ethash' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Etchash'), 'etchash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KawPoW'), 'kawpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KHeavyHash (Kaspa)'), 'kaspa' UNION ALL
    SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KarlsenHash'), 'karlsenhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA-512-256D (Radiant)'), 'sha512_256d_radiant' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Argon2d-16000'), 'argon2d_16000' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Aurum'), 'aurum' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'argon2id_chukwa' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake (2b)'), 'blake2b' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake (2s)'), 'blake2s' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'blake3_alephium' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (IronFish)'), 'blake3_ironfish' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightGPU'), 'cryptonight_gpu' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightTalleo'), 'cryptonight_talleo' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CryptoNightTurtle'), 'cryptonight_turtle' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Curvehash'), 'curvehash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CpuPower'), 'cpupower' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ETHash-B3'), 'ethashb3' UNION ALL
    SELECT monitoring_center.get_algorithm_id('FiroPoW'), 'firopow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('GhostRider (Raptoreum)'), 'ghostrider' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HeavyHash'), 'heavyhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Keccak'), 'keccak' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Memehash'), 'memehash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Memehash (ApepePow)'), 'memehash_apepepow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Mike'), 'mike' UNION ALL
    SELECT monitoring_center.get_algorithm_id('MinotaurX'), 'minotaurx' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Panthera'), 'panthera' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowSERO'), 'progpow_sero' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ProgPowVeil'), 'progpow_veil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('PufferFish2BMB'), 'pufferfish2bmb' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomHash2'), 'randomhash2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomSFX'), 'randomsfx' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomTuske'), 'randomtuske' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomX'), 'randomx' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomARQ'), 'randomarq' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomEPIC'), 'randomepic' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomNEVO'), 'randomnevo' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomSCASH'), 'randomscash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomYADA'), 'randomyada' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomKEVA'), 'randomkeva' UNION ALL
    SELECT monitoring_center.get_algorithm_id('RandomGRAFT'), 'randomgrft' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA256DT'), 'sha256dt' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA3d'), 'sha3d' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ubqhash'), 'ubqhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Verthash'), 'verthash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('VerusHash'), 'verushash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xelis (Xel)'), 'xelishash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xelis V2 (Xel)'), 'xelishashv2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Yescrypt'), 'yescrypt' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YescryptR16'), 'yescryptr16' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YescryptR32'), 'yescryptr32' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YescrptR8'), 'yescryptr8' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YesPoWer'), 'yespower' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YesPoWerR16'), 'yespowerr16'
) f union all

select monitoring_center.get_miner_id('Rigel', '1.20.1'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('Ethash') algorithm_id, 'ethash' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Etchash'), 'etchash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KawPoW'), 'kawpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'alephium' UNION ALL
    SELECT monitoring_center.get_algorithm_id('FishHash'), 'fishhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Gram'), 'sha256ton' UNION ALL
    SELECT monitoring_center.get_algorithm_id('ETHash-B3'), 'ethashb3' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (IronFish)'), 'ironfish' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KarlsenHash'), 'karlsenhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('NexaPoW'), 'nexapow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'pyrinhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA-512-256D (Radiant)'), 'sha512256d' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Octopus'), 'octopus' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xelis (Xel)'), 'xelishash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xelis V2 (Xel)'), 'xelishashv2'
) f union all

select monitoring_center.get_miner_id('OneZeroMiner', '1.4.3'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('DynexSolve') algorithm_id, 'dynex' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xelis (Xel)'), 'xelis' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Xelis V2 (Xel)'), 'xelishashv2'
) f;

select monitoring_center.get_miner_id('LolMiner', '1.94a'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('Ethash') algorithm_id, 'ETHASH' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Etchash'), 'ETCHASH' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'AUTOLYKOS2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Equihash(144,5)'), 'EQUI144_5' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Equihash(210,9)'), 'EQUI210_9' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CuckooCycle (Cuckoo29)'), 'C29AE' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (Alephium)'), 'ALEPH' UNION ALL
    SELECT monitoring_center.get_algorithm_id('BeamHash III'), 'BEAM-III' UNION ALL
    SELECT monitoring_center.get_algorithm_id('cuckAToo31'), 'C31' UNION ALL
    SELECT monitoring_center.get_algorithm_id('cuckAToo32'), 'C32' UNION ALL
    SELECT monitoring_center.get_algorithm_id('FishHash'), 'FISHHASH' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Gram'), 'GRAM' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Equihash(192,7)'), 'EQUI192_7' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Blake3 (IronFish)'), 'IRONFISH' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KarlsenHash'), 'KARLSEN' UNION ALL
    SELECT monitoring_center.get_algorithm_id('NexaPoW'), 'NEXA' UNION ALL
    SELECT monitoring_center.get_algorithm_id('HeavyHash-Pyrin (Pyrin, Pyrinhash)'), 'PYRIN' UNION ALL
    SELECT monitoring_center.get_algorithm_id('SHA-512-256D (Radiant)'), 'RADIANT' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil'
) f union all

select monitoring_center.get_miner_id('GMiner', '3.44'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('Ethash') algorithm_id, 'ethash' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Etchash'), 'etchash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KawPoW'), 'kawpow' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Autolykos2'), 'autolykos2' UNION ALL
    SELECT monitoring_center.get_algorithm_id('KHeavyHash (Kaspa)'), 'kheavyhash' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Cortex'), 'cortex' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Equihash(144,5)'), 'quihash144_5' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Equihash(125,4)'), 'equihash125_4' UNION ALL
    SELECT monitoring_center.get_algorithm_id('Equihash(210,9)'), 'equihash210_9' UNION ALL
    SELECT monitoring_center.get_algorithm_id('CuckooCycle (Cuckoo29)'), 'cuckoo29'
) f;

-- ########################## DELETE FUNCTIONS ##############################

drop function monitoring_center.get_miner_id(miner_name text, miner_version text);
drop function monitoring_center.get_algorithm_id(algorithm_name text);
