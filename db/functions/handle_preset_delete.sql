CREATE OR REPLACE FUNCTION monitoring_center.handle_preset_delete() RETURNS TRIGGER AS $$
BEGIN
	UPDATE monitoring_center.mining_devices
	SET 
		preset_id = (
			SELECT id
			FROM monitoring_center.presets
			WHERE name = monitoring_center.mining_devices.id::text AND is_visible = false
			LIMIT 1
		)
	WHERE preset_id = OLD.id;

	UPDATE monitoring_center.overclocking
	SET
		core_clock_lock = (SELECT core_clock_lock FROM monitoring_center.presets WHERE id = OLD.id),
        core_clock_offset = (SELECT core_clock_offset FROM monitoring_center.presets WHERE id = OLD.id),
        memory_clock_lock = (SELECT memory_clock_lock FROM monitoring_center.presets WHERE id = OLD.id),
        memory_clock_offset = (SELECT memory_clock_offset FROM monitoring_center.presets WHERE id = OLD.id),
        core_voltage = (SELECT core_voltage FROM monitoring_center.presets WHERE id = OLD.id),
        core_voltage_offset = (SELECT core_voltage_offset FROM monitoring_center.presets WHERE id = OLD.id),
        memory_voltage = (SELECT memory_voltage FROM monitoring_center.presets WHERE id = OLD.id),
        memory_voltage_offset = (SELECT memory_voltage_offset FROM monitoring_center.presets WHERE id = OLD.id),
        power_limit = (SELECT power_limit FROM monitoring_center.presets WHERE id = OLD.id),
        fan_speed = (SELECT fan_speed FROM monitoring_center.presets WHERE id = OLD.id)
	WHERE id = (SELECT overclocking_id FROM monitoring_center.presets WHERE id = OLD.id);

	DELETE FROM monitoring_center.overclocking
    WHERE id = (SELECT overclocking_id FROM monitoring_center.presets WHERE id = OLD.id);

	RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER preset_delete_trigger
BEFORE DELETE ON monitoring_center.presets
FOR EACH ROW
EXECUTE FUNCTION monitoring_center.handle_preset_delete();