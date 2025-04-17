insert into monitoring_center.pools(domain, port, cryptocurrency_id) values

-- ========== Ethereum Classic ==========
('etc.f2pool.com',                8008,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ethereum Classic')),
('eu.etc.k1pool.com',             3821,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ethereum Classic')),
('etc.2miners.com',               1010,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ethereum Classic')),

-- ========== Ergo ==========
('erg.2miners.com',               8888,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ergo')),
('ru.ergo.herominers.com',        1180,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ergo')),
('erg.ss.dxpool.com',             8888,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ergo')),
('ergo-eu1.nanopool.org',         10600, (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ergo')),

-- ========== Blocx ==========
('pool.eu.woolypooly.com',        3148,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Blocx')),
('blocx-eu.kryptex.network',      7020,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Blocx')),
('blocx.kryptex.network',         7777,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Blocx')),
('europe.thepool.zone',           3368,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Blocx')),

-- ========== VertCoin ==========
('pool.ru.woolypooly.com',        3102,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'VertCoin')),
('verthash.eu.mine.zergpool.com', 4534,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'VertCoin')),

-- ========== Conceal ==========
('pool.hashvault.pro',            443,   (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Conceal')),
('conceal.cedric-crispin.com',    3364,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Conceal')),

-- ========== Zano ==========
('zano.luckypool.io',             8866,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Zano')),
('zano.luckypool.io',             8877,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Zano')),
('pool.ru.woolypooly.com',        3146,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Zano')),

-- ========== Dynex ==========
('ru.dynex.herominers.com',       1120,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Dynex')),
('pool.deepminerz.com',           3333,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Dynex')),
('pool.deepminerz.com',           4444,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Dynex')),
('dnx.eu.neuropool.net',          19330, (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Dynex')),
('dnx.eu.neuropool.net',          19331, (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Dynex')),
('eu.dnx.k1pool.com',             3690,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Dynex')),

-- ========== Cortex ==========
('ctxc.2miners.com',              2222,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Cortex')),
('ru.cortex.herominers.com',      1155,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Cortex')),
 
-- ========== Xelis ========== 
('ru.xelis.herominers.com',       1225,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('eu.xel.k1pool.com',             9351,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('ru.vipor.net',                  5077,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('fi.grandpool.io',               2025,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('fi.grandpool.io',               2026,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('fr.grandpool.io',               2025,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('fr.grandpool.io',               2026,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('ua.grandpool.io',               2025,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
('ua.grandpool.io',               2026,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Xelis')),
 
-- ========== Karlsen ========== 
('ru.karlsen.herominers.com',     1195,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Karlsen')),
('pool.ru.woolypooly.com',        3132,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Karlsen')),
('kls-eu.kryptex.network',        7022,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Karlsen')),
('kls.kryptex.network',           7777,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Karlsen')),
 
-- ========== Conflux ========== 
('ru.conflux.herominers.com',     1170,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Conflux')),
('cfx-eu.kryptex.network',        7027,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Conflux')),
('cfx-eu1.nanopool.org',          10500, (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Conflux')),
 
-- ========== Iron Fish ========= =
('ru.ironfish.herominers.com',    1145,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Iron Fish')),
('iron-eu.kryptex.network',       7017,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Iron Fish')),
 
-- ========== Evrmore ========== 
('eu.evrpool.org',                1111,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Evrmore')),
('eu.mining4people.com',          4173,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Evrmore')),
 
-- ========== Clore ========== 
('clore.2miners.com',             2020,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Clore')),
('ru.clore.herominers.com',       1163,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Clore')),
('stratum-eu.rplant.xyz',         7083,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Clore')),
('ru.vipor.net',                  5030,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Clore')),
('eu.clore.k1pool.com',           5030,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Clore')),
 
-- ========== MeowCoin ========== 
('pool.ru.woolypooly.com',        3116,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'MeowCoin')),
('stratum-eu.rplant.xyz',         7120,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'MeowCoin')),
('eu-stratum.blockminerz.com',    3308,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'MeowCoin')),
('meowpow.eu.mine.zergpool.com',  3640,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'MeowCoin')),
 
-- ========== Grin ========== 
('europe.pool.easygrin.org',      3001,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Grin')),
('grin.2miners.com',              3030,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Grin')),
 
-- ========== Ravencoin ========= =
('rvn.2miners.com',               6060,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ravencoin')),
 
-- ========== Firo ========== 
('pool.ru.woolypooly.com',        3104,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Firo')),
 
-- ========== Neurai ========== 
('xna.2miners.com',               6060,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Neurai')),
('ru.neurai.herominers.com',      1160,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Neurai')),
 
-- ========== NEOXA ========== 
('neox.2miners.com',              4040,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'NEOXA')),
('ru.neoxa.herominers.com',       1202,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'NEOXA')),
 
-- ========== Ethereum PoW ====== ====
('ethw.f2pool.com',               6688,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ethereum PoW')),
('ethw.2miners.com',              2020,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ethereum PoW')),
('eu.ethw.k1pool.com',            7691,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Ethereum PoW')),
 
-- ========== Gemlink ========== 
('eu.equihub.pro',                3033,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Gemlink')),
('eu.equihub.pro',                3034,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Gemlink')),
('glink.us.2mars.biz',            8888,  (SELECT id FROM monitoring_center.cryptocurrencies WHERE name = 'Gemlink'));