\set ON_ERROR_STOP 1
\set ECHO all

-- CREATING SCHEMA
\ir 'schemas/monitoring_center.sql'

-- CREATING TABLES
\ir 'tables/inventory.sql'
\ir 'tables/cpu.sql'
\ir 'tables/drive.sql'
\ir 'tables/gpu.sql'
\ir 'tables/motherboard.sql'
\ir 'tables/motherboard_pci.sql'
\ir 'tables/network_adapter.sql'
\ir 'tables/software_inventory.sql'
\ir 'tables/version_info.sql'

-- DATA INSERT
\ir 'data_tables/version_info.sql'
