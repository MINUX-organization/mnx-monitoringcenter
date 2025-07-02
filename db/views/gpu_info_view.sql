CREATE OR REPLACE VIEW monitoring_center.gpu_info_view AS
SELECT
	monitoring_center.get_gpu_driver_version(
		software_inventory.amd_gpu_driver_version,
        software_inventory.intel_gpu_driver_version,
        software_inventory.nvidia_gpu_driver_version,
        gpu.manufacturer
	) AS driver_version,
	rigs.name AS rig_name,
	gpu.* END
FROM monitoring_center.gpu
JOIN monitoring_center.rig_inventory ON gpu.rig_inventory_id = rig_inventory.id
JOIN monitoring_center.rigs ON rig_inventory.rig_id = rigs.id
JOIN monitoring_center.software_inventory ON software_inventory.rig_inventory_id = rig_inventory.id;