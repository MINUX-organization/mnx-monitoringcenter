\set ON_ERROR_STOP 1
\set ECHO all

-- CREATING SCHEMA
\ir 'schemas/monitoring_center.sql'

-- CREATING TYPES
\ir 'types/mining_mode.sql'

-- CREATING TABLES
\ir 'tables/algorithms.sql'
\ir 'tables/cryptocurrencies.sql'
\ir 'tables/miners.sql'
\ir 'tables/overclocking.sql'
\ir 'tables/pools.sql'
\ir 'tables/presets.sql'
\ir 'tables/wallets.sql'
\ir 'tables/flight_sheets.sql'
\ir 'tables/flight_sheet_targets.sql'
\ir 'tables/flight_sheet_target_configs.sql'

\ir 'tables/inventory.sql'
\ir 'tables/cpu.sql'
\ir 'tables/drive.sql'
\ir 'tables/gpu.sql'
\ir 'tables/motherboard.sql'
\ir 'tables/motherboard_pci.sql'
\ir 'tables/network_adapter.sql'
\ir 'tables/software_inventory.sql'

\ir 'tables/version_info.sql'

-- CREATING FUNCTIONS
\ir 'functions/get_gpu_driver_version.sql'

-- DATA INSERT
\ir 'data_tables/version_info.sql'
