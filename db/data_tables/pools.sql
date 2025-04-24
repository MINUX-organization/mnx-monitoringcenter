insert into monitoring_center.pools(domain, port, cryptocurrency_id) values

-- ========== Ethereum Classic ==========
('etc.f2pool.com',                8008,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ethereum Classic')),
('eu.etc.k1pool.com',             3821,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ethereum Classic')),
('etc.2miners.com',               1010,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ethereum Classic')),

-- ========== Ergo ==========
('erg.2miners.com',               8888,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ergo')),
('ru.ergo.herominers.com',        1180,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ergo')),
('erg.ss.dxpool.com',             8888,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ergo')),
('ergo-eu1.nanopool.org',         10600, (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ergo')),

-- ========== Blocx ==========
('pool.eu.woolypooly.com',        3148,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Blocx')),
('blocx-eu.kryptex.network',      7020,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Blocx')),
('blocx.kryptex.network',         7777,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Blocx')),
('europe.thepool.zone',           3368,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Blocx')),

-- ========== VertCoin ==========
('pool.ru.woolypooly.com',        3102,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'VertCoin')),
('verthash.eu.mine.zergpool.com', 4534,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'VertCoin')),

-- ========== Conceal ==========
('pool.hashvault.pro',            443,   (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Conceal')),
('conceal.cedric-crispin.com',    3364,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Conceal')),

-- ========== Zano ==========
('zano.luckypool.io',             8866,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Zano')),
('zano.luckypool.io',             8877,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Zano')),
('pool.ru.woolypooly.com',        3146,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Zano')),

-- ========== Dynex ==========
('ru.dynex.herominers.com',       1120,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Dynex')),
('pool.deepminerz.com',           3333,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Dynex')),
('pool.deepminerz.com',           4444,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Dynex')),
('dnx.eu.neuropool.net',          19330, (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Dynex')),
('dnx.eu.neuropool.net',          19331, (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Dynex')),
('eu.dnx.k1pool.com',             3690,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Dynex')),

-- ========== Cortex ==========
('ctxc.2miners.com',              2222,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Cortex')),
('ru.cortex.herominers.com',      1155,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Cortex')),
 
-- ========== Xelis ========== 
('ru.xelis.herominers.com',       1225,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('eu.xel.k1pool.com',             9351,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('ru.vipor.net',                  5077,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('fi.grandpool.io',               2025,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('fi.grandpool.io',               2026,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('fr.grandpool.io',               2025,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('fr.grandpool.io',               2026,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('ua.grandpool.io',               2025,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
('ua.grandpool.io',               2026,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Xelis')),
 
-- ========== Karlsen ========== 
('ru.karlsen.herominers.com',     1195,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Karlsen')),
('pool.ru.woolypooly.com',        3132,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Karlsen')),
('kls-eu.kryptex.network',        7022,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Karlsen')),
('kls.kryptex.network',           7777,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Karlsen')),
 
-- ========== Conflux ========== 
('ru.conflux.herominers.com',     1170,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Conflux')),
('cfx-eu.kryptex.network',        7027,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Conflux')),
('cfx-eu1.nanopool.org',          10500, (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Conflux')),
 
-- ========== Iron Fish ========= =
('ru.ironfish.herominers.com',    1145,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Iron Fish')),
('iron-eu.kryptex.network',       7017,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Iron Fish')),
 
-- ========== Evrmore ========== 
('eu.evrpool.org',                1111,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Evrmore')),
('eu.mining4people.com',          4173,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Evrmore')),
 
-- ========== Clore ========== 
('clore.2miners.com',             2020,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Clore')),
('ru.clore.herominers.com',       1163,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Clore')),
('stratum-eu.rplant.xyz',         7083,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Clore')),
('ru.vipor.net',                  5030,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Clore')),
('eu.clore.k1pool.com',           5030,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Clore')),
 
-- ========== MeowCoin ========== 
('pool.ru.woolypooly.com',        3116,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'MeowCoin')),
('stratum-eu.rplant.xyz',         7120,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'MeowCoin')),
('eu-stratum.blockminerz.com',    3308,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'MeowCoin')),
('meowpow.eu.mine.zergpool.com',  3640,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'MeowCoin')),
 
-- ========== Grin ========== 
('europe.pool.easygrin.org',      3001,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Grin')),
('grin.2miners.com',              3030,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Grin')),
 
-- ========== Ravencoin ========= =
('rvn.2miners.com',               6060,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ravencoin')),
 
-- ========== Firo ========== 
('pool.ru.woolypooly.com',        3104,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Firo')),
 
-- ========== Neurai ========== 
('xna.2miners.com',               6060,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Neurai')),
('ru.neurai.herominers.com',      1160,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Neurai')),
 
-- ========== NEOXA ========== 
('neox.2miners.com',              4040,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'NEOXA')),
('ru.neoxa.herominers.com',       1202,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'NEOXA')),
 
-- ========== Ethereum PoW ====== ====
('ethw.f2pool.com',               6688,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ethereum PoW')),
('ethw.2miners.com',              2020,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ethereum PoW')),
('eu.ethw.k1pool.com',            7691,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Ethereum PoW')),
 
-- ========== Gemlink ========== 
('eu.equihub.pro',                3033,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Gemlink')),
('eu.equihub.pro',                3034,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Gemlink')),
('glink.us.2mars.biz',            8888,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE full_name = 'Gemlink'));