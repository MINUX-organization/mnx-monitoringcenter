CREATE FUNCTION monitoring_center.get_gpu_driver_version (
    amd_driver_version text,
    intel_driver_version text,
    nvidia_driver_version text,
    gpu_manufacturer text
)
RETURNS text
AS
$BODY$
DECLARE
    driver_version text;

BEGIN
    driver_version := CASE 
        WHEN gpu_manufacturer = 'AMD' THEN amd_driver_version
        WHEN gpu_manufacturer = 'Intel' THEN intel_driver_version
        WHEN gpu_manufacturer = 'Nvidia' THEN nvidia_driver_version
        ELSE NULL
    END;

    RETURN driver_version;
END;
$BODY$
LANGUAGE plpgsql;