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

select monitoring_center.get_miner_id('rigel', '1.20.1'), f.algorithm_id, f.name from
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

select monitoring_center.get_miner_id('gminer', '3.44'), f.algorithm_id, f.name from
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

select monitoring_center.get_miner_id('xmrig', '6.22.2'), f.algorithm_id, f.name from
(
    select monitoring_center.get_algorithm_id('GhostRider (Raptoreum)') algorithm_id, 'gr' name union all
    select monitoring_center.get_algorithm_id('RandomGRAFT'), 'rx/graft' union all
    select monitoring_center.get_algorithm_id('CryptoNight Femto'), 'cn/upx2' union all
    select monitoring_center.get_algorithm_id('CryptoNight Conceal (CCX)'), 'cn/ccx' union all
    select monitoring_center.get_algorithm_id('RandomKEVA'), 'rx/keva' union all
    select monitoring_center.get_algorithm_id('CryptoNight Talleo'), 'cn-pico/tlo' union all
    select monitoring_center.get_algorithm_id('RandomSFX'), 'rx/sfx' union all
    select monitoring_center.get_algorithm_id('RandomARQ'), 'rx/arq' union all
    select monitoring_center.get_algorithm_id('RandomX'), 'rx/0' union all
    select monitoring_center.get_algorithm_id('Chukwa (Argon2id-Chuckwa)'), 'argon2/chukwa' union all
    select monitoring_center.get_algorithm_id('NINJA (Argon2id-NINJA)'), 'argon2/ninja' union all
    select monitoring_center.get_algorithm_id('RandomWow'), 'rx/wow' union all
    select monitoring_center.get_algorithm_id('CryptoNight Pico'), 'cn-pico' union all
    select monitoring_center.get_algorithm_id('CryptoNight R (CryptonightV4)'), 'cn/r'
) f union all

select monitoring_center.get_miner_id('wildrig-multi', '0.42.2'), f.algorithm_id, f.name from
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

select monitoring_center.get_miner_id('t-rex', '0.26.8'), f.algorithm_id, f.name from
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

select monitoring_center.get_miner_id('teamredminer', '0.10.21'), f.algorithm_id, f.name from
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
    SELECT monitoring_center.get_algorithm_id('X16S'), 'x16s' UNION ALL
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

select monitoring_center.get_miner_id('SRBMiner-MULTI', '2.7.6'), f.algorithm_id, f.name from
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
    SELECT monitoring_center.get_algorithm_id('RandomARQ'), 'randomarq' UNION ALL
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
    SELECT monitoring_center.get_algorithm_id('Aurum'), 'aurum' UNION ALL
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
    SELECT monitoring_center.get_algorithm_id('Yespower2b'), 'yespower2b' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YespowerSugar'), 'yespowersugar' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YespowerUrx'), 'yespowerurx' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YespowerLtncg'), 'yespowerltncg' UNION ALL
    SELECT monitoring_center.get_algorithm_id('YespowerR16'), 'yespowerr16'
) f union all

select monitoring_center.get_miner_id('onezerominer', '1.4.3'), f.algorithm_id, f.name from
(
    SELECT monitoring_center.get_algorithm_id('DynexSolve') algorithm_id, 'dynex' name UNION ALL
    SELECT monitoring_center.get_algorithm_id('Ziliqa (ZIL)'), 'zil' UNION ALL
    SELECT monitoring_center.get_algorithm_id('XelisHash (Xel)'), 'xelis' UNION ALL
    SELECT monitoring_center.get_algorithm_id('XelisHashV2 (Xel)'), 'xelishashv2'
) f union all

select monitoring_center.get_miner_id('lolMiner', '1.94a'), f.algorithm_id, f.name from
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
    SELECT monitoring_center.get_algorithm_id('Equihash(192,7)'), 'EQUI192_7' UNION ALL
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

select monitoring_center.get_miner_id('deroluna-miner', '1.14'), f.algorithm_id, f.name from
(
    select monitoring_center.get_algorithm_id('AstroBWTv3') algorithm_id, 'astrobwt' name
) f;

-- ########################## DELETE FUNCTIONS ##############################

drop function monitoring_center.get_miner_id(miner_name text, miner_version text);
drop function monitoring_center.get_algorithm_id(algorithm_name text);
