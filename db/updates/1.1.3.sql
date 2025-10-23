\set ON_ERROR_STOP 1
\set ECHO all
\encoding UTF8

DO $outer$
DECLARE
current_version TEXT;
    expected_version TEXT := '1.1.2'; 
    new_version TEXT := '1.1.3'; 
                
BEGIN

    SELECT version INTO current_version FROM monitoring_center.version_info;
    IF current_version != expected_version THEN
            RAISE EXCEPTION 'The database version % does not match the expected version %', current_version, expected_version;
    END IF;

    ALTER TABLE monitoring_center.rigs
    ADD COLUMN is_decommissioned boolean NOT NULL DEFAULT FALSE;
    COMMENT ON COLUMN monitoring_center.rigs.is_decommissioned IS 'Статус вывода рига из эксплуатации.';
    
    UPDATE monitoring_center.version_info SET version = new_version;

END$outer$ LANGUAGE plpgsql;