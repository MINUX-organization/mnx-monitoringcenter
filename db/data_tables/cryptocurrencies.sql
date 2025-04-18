insert into monitoring_center.cryptocurrencies(short_name, full_name, algorithm_id) values

-- ========== EtcHash ==========
('ETC',   'Ethereum Classic', (SELECT id FROM monitoring_center.algorithms WHERE name = 'EtcHash')),

-- ========== Autolykos2 ==========
('ERGO',  'Ergo',             (SELECT id FROM monitoring_center.algorithms WHERE name = 'Autolykos2')),
('BLOCX', 'Blocx',            (SELECT id FROM monitoring_center.algorithms WHERE name = 'Autolykos2')),

-- ========== Verthash ==========
('VTC',   'VertCoin',         (SELECT id FROM monitoring_center.algorithms WHERE name = 'Verthash')),

-- ========== CryptoNight GPU ==========
('CCX',   'Conceal',          (SELECT id FROM monitoring_center.algorithms WHERE name = 'CryptoNight GPU')),

-- ========== ProgPowZ ==========
('ZANO',  'Zano',             (SELECT id FROM monitoring_center.algorithms WHERE name = 'ProgPowZ')),

-- ========== DynexSolve ==========
('DNX',   'Dynex',            (SELECT id FROM monitoring_center.algorithms WHERE name = 'DynexSolve')),

-- ========== Cortex ==========
('CTXC',  'Cortex',           (SELECT id FROM monitoring_center.algorithms WHERE name = 'Cortex')),

-- ========== XelisHashV2 (Xel) ==========
('XEL',   'Xelis',            (SELECT id FROM monitoring_center.algorithms WHERE name = 'XelisHashV2 (Xel)')),

-- ========== FishHash ==========
('KLS',   'Karlsen',          (SELECT id FROM monitoring_center.algorithms WHERE name = 'FishHash')),
('IRON',  'Iron Fish',        (SELECT id FROM monitoring_center.algorithms WHERE name = 'FishHash')),

-- ========== Octopus ==========
('CFX',   'Conflux',          (SELECT id FROM monitoring_center.algorithms WHERE name = 'Octopus')),

-- ========== EvrProgPow ==========
('EVR',   'Evrmore',          (SELECT id FROM monitoring_center.algorithms WHERE name = 'EvrProgPow')),

-- ========== Kawpow ==========
('CLORE', 'Clore',            (SELECT id FROM monitoring_center.algorithms WHERE name = 'Kawpow')),
('RVN',   'Ravencoin',        (SELECT id FROM monitoring_center.algorithms WHERE name = 'Kawpow')),
('XNA',   'Neurai',           (SELECT id FROM monitoring_center.algorithms WHERE name = 'Kawpow')),
('NEOX',  'NEOXA',            (SELECT id FROM monitoring_center.algorithms WHERE name = 'Kawpow')),

-- ========== Meowpow ==========
('MEWC',  'MeowCoin',         (SELECT id FROM monitoring_center.algorithms WHERE name = 'Meowpow')),

-- ========== Cuckatoo32 ==========
('GRIN',  'Grin',             (SELECT id FROM monitoring_center.algorithms WHERE name = 'CuckaToo32')),

-- ========== FiroPow ==========
('FIRO',  'Firo',             (SELECT id FROM monitoring_center.algorithms WHERE name = 'FiroPow')),

-- ========== Ethash (Ethereum) ==========
('ETHW',  'Ethereum PoW',     (SELECT id FROM monitoring_center.algorithms WHERE name = 'Ethash (Ethereum)')),

-- ========== Zhash (Equihash 144,5) ==========
('GLINK', 'Gemlink',          (SELECT id FROM monitoring_center.algorithms WHERE name = 'Zhash (Equihash 144,5)'));