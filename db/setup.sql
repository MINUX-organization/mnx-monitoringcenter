\set ON_ERROR_STOP 1
\set ECHO all

-- CREATING SCHEMA
\ir 'schemas/monitoring_center.sql'

-- CREATING TABLES
\ir 'tables/algorithms.sql'
\ir 'tables/cryptocurrencies.sql'
\ir 'tables/miners.sql'
\ir 'tables/overclocking.sql'
\ir 'tables/pools.sql'
\ir 'tables/presets.sql'
\ir 'tables/version_info.sql'
\ir 'tables/wallets.sql'

-- DATA INSERT
\ir 'data_tables/version_info.sql'
