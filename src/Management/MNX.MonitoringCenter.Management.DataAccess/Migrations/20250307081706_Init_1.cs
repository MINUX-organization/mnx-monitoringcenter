using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MNX.MonitoringCenter.Management.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MiningDevices_GpuOverclocking_PresetId",
                table: "MiningDevices");

            migrationBuilder.DropTable(
                name: "GpuOverclocking");

            migrationBuilder.AddForeignKey(
                name: "FK_MiningDevices_Presets_PresetId",
                table: "MiningDevices",
                column: "PresetId",
                principalTable: "Presets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MiningDevices_Presets_PresetId",
                table: "MiningDevices");

            migrationBuilder.CreateTable(
                name: "GpuOverclocking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoreClockLock = table.Column<int>(type: "integer", nullable: false),
                    CoreClockOffset = table.Column<int>(type: "integer", nullable: false),
                    CoreVoltage = table.Column<int>(type: "integer", nullable: false),
                    CoreVoltageOffset = table.Column<int>(type: "integer", nullable: false),
                    FanSpeed = table.Column<int>(type: "integer", nullable: false),
                    MemoryClockLock = table.Column<int>(type: "integer", nullable: false),
                    MemoryClockOffset = table.Column<int>(type: "integer", nullable: false),
                    MemoryVoltage = table.Column<int>(type: "integer", nullable: false),
                    MemoryVoltageOffset = table.Column<int>(type: "integer", nullable: false),
                    PowerLimit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpuOverclocking", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MiningDevices_GpuOverclocking_PresetId",
                table: "MiningDevices",
                column: "PresetId",
                principalTable: "GpuOverclocking",
                principalColumn: "Id");
        }
    }
}
