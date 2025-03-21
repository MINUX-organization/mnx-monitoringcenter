CREATE TABLE monitoring_center.presets
(
    id uuid NOT NULL,
    name text NOT NULL,
    device_name text NOT NULL,
    overclocking_id uuid NOT NULL,
    user_id uuid NOT NULL,
    is_visible boolean NOT NULL,

    CONSTRAINT pk_presets PRIMARY KEY (id),

    CONSTRAINT fk_presets_overclocking_overclocking_id FOREIGN KEY (overclocking_id)
        REFERENCES monitoring_center.overclocking (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX ix_presets_overclocking_id
    ON monitoring_center.presets USING btree
    (overclocking_id ASC NULLS LAST);

CREATE INDEX ix_presets_user_id
    ON monitoring_center.presets USING btree
    (user_id ASC NULLS LAST);

COMMENT ON TABLE monitoring_center.presets IS 'Пресеты';

CREATE OR REPLACE FUNCTION handle_preset_delete() RETURNS TRIGGER AS $$
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
EXECUTE FUNCTION handle_preset_delete();