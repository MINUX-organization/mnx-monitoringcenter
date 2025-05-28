\set ON_ERROR_STOP 1
\set ECHO all
\encoding UTF8

-- CREATING SCHEMA
\ir 'schemas/monitoring_center.sql'

-- CREATING EXTENSIONS
\ir 'extensions/guid_generator_extension.sql'

-- CREATING TABLES
\ir 'tables/algorithms.sql'
\ir 'tables/cryptocurrencies.sql'
\ir 'tables/miners.sql'
\ir 'tables/miner_algorithms.sql'
\ir 'tables/overclocking.sql'
\ir 'tables/pools.sql'
\ir 'tables/presets.sql'
\ir 'tables/wallets.sql'
\ir 'tables/flight_sheets.sql'
\ir 'tables/flight_sheet_targets.sql'
\ir 'tables/mining_coin_configs.sql'
\ir 'tables/mining_devices.sql'

\ir 'tables/rigs.sql'
\ir 'tables/rig_inventory.sql'
\ir 'tables/cpu.sql'
\ir 'tables/drive.sql'
\ir 'tables/gpu.sql'
\ir 'tables/motherboard.sql'
\ir 'tables/motherboard_pci.sql'
\ir 'tables/network_adapter.sql'
\ir 'tables/software_inventory.sql'
\ir 'tables/miners_inventory.sql'

\ir 'tables/version_info.sql'

-- CREATING FUNCTIONS
\ir 'functions/get_gpu_driver_version.sql'
\ir 'functions/handle_preset_delete.sql'

-- DATA INSERT
\ir 'data_tables/algorithms.sql'
\ir 'data_tables/miners.sql'
\ir 'data_tables/miner_algorithms.sql'
\ir 'data_tables/cryptocurrencies.sql'
\ir 'data_tables/pools.sql'
\ir 'data_tables/version_info.sql'
