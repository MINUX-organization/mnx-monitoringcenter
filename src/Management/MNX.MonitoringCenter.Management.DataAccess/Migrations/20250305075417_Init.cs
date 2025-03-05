using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MNX.MonitoringCenter.Management.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Algorithms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Algorithms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightSheets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSheets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GpuOverclocking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CoreClockLock = table.Column<int>(type: "integer", nullable: false),
                    CoreClockOffset = table.Column<int>(type: "integer", nullable: false),
                    MemoryClockLock = table.Column<int>(type: "integer", nullable: false),
                    MemoryClockOffset = table.Column<int>(type: "integer", nullable: false),
                    CoreVoltage = table.Column<int>(type: "integer", nullable: false),
                    CoreVoltageOffset = table.Column<int>(type: "integer", nullable: false),
                    MemoryVoltage = table.Column<int>(type: "integer", nullable: false),
                    MemoryVoltageOffset = table.Column<int>(type: "integer", nullable: false),
                    PowerLimit = table.Column<int>(type: "integer", nullable: false),
                    FanSpeed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpuOverclocking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Miners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    SupportedDevices = table.Column<int>(type: "integer", nullable: false),
                    MiningMode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Overclocking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetDeviceType = table.Column<string>(type: "text", nullable: false),
                    CoreClockLock = table.Column<int>(type: "integer", nullable: true),
                    CoreClockOffset = table.Column<int>(type: "integer", nullable: true),
                    MemoryClockLock = table.Column<int>(type: "integer", nullable: true),
                    MemoryClockOffset = table.Column<int>(type: "integer", nullable: true),
                    CoreVoltage = table.Column<int>(type: "integer", nullable: true),
                    CoreVoltageOffset = table.Column<int>(type: "integer", nullable: true),
                    MemoryVoltage = table.Column<int>(type: "integer", nullable: true),
                    MemoryVoltageOffset = table.Column<int>(type: "integer", nullable: true),
                    PowerLimit = table.Column<int>(type: "integer", nullable: true),
                    FanSpeed = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Overclocking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Presets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DeviceName = table.Column<string>(type: "text", nullable: false),
                    OverclockingId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cryptocurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    AlgorithmId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptocurrencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cryptocurrencies_Algorithms_AlgorithmId",
                        column: x => x.AlgorithmId,
                        principalTable: "Algorithms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiningDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RigId = table.Column<Guid>(type: "uuid", nullable: false),
                    LifeCycleStatus = table.Column<string>(type: "text", nullable: false),
                    FlightSheetId = table.Column<Guid>(type: "uuid", nullable: true),
                    FlightSheetIsConfirm = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    PresetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiningDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiningDevices_GpuOverclocking_PresetId",
                        column: x => x.PresetId,
                        principalTable: "GpuOverclocking",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlightSheetTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightSheetId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: false),
                    AdditionalArguments = table.Column<string>(type: "text", nullable: true),
                    ConfigFileContent = table.Column<string>(type: "text", nullable: true),
                    MinerId = table.Column<Guid>(type: "uuid", nullable: false),
                    HugePages = table.Column<int>(type: "integer", nullable: true),
                    ThreadsCount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSheetTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightSheetTargets_FlightSheets_FlightSheetId",
                        column: x => x.FlightSheetId,
                        principalTable: "FlightSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightSheetTargets_Miners_MinerId",
                        column: x => x.MinerId,
                        principalTable: "Miners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MinerAlgorithms",
                columns: table => new
                {
                    AlgorithmId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinerAlgorithms", x => new { x.MinerId, x.AlgorithmId });
                    table.ForeignKey(
                        name: "FK_MinerAlgorithms_Miners_MinerId",
                        column: x => x.MinerId,
                        principalTable: "Miners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Domain = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    CryptocurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pools_Cryptocurrencies_CryptocurrencyId",
                        column: x => x.CryptocurrencyId,
                        principalTable: "Cryptocurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CryptocurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Cryptocurrencies_CryptocurrencyId",
                        column: x => x.CryptocurrencyId,
                        principalTable: "Cryptocurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiningCoinConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    PoolPassword = table.Column<string>(type: "text", nullable: true),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    flight_sheet_target_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiningCoinConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiningCoinConfigs_FlightSheetTargets_flight_sheet_target_id",
                        column: x => x.flight_sheet_target_id,
                        principalTable: "FlightSheetTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiningCoinConfigs_Pools_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiningCoinConfigs_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Algorithms_UserId_Name",
                table: "Algorithms",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cryptocurrencies_AlgorithmId",
                table: "Cryptocurrencies",
                column: "AlgorithmId");

            migrationBuilder.CreateIndex(
                name: "IX_Cryptocurrencies_FullName",
                table: "Cryptocurrencies",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Cryptocurrencies_ShortName",
                table: "Cryptocurrencies",
                column: "ShortName");

            migrationBuilder.CreateIndex(
                name: "IX_Cryptocurrencies_UserId",
                table: "Cryptocurrencies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_Name",
                table: "FlightSheets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheets_UserId",
                table: "FlightSheets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheetTargets_FlightSheetId",
                table: "FlightSheetTargets",
                column: "FlightSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSheetTargets_MinerId",
                table: "FlightSheetTargets",
                column: "MinerId");

            migrationBuilder.CreateIndex(
                name: "IX_Miners_Name",
                table: "Miners",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MiningCoinConfigs_flight_sheet_target_id",
                table: "MiningCoinConfigs",
                column: "flight_sheet_target_id");

            migrationBuilder.CreateIndex(
                name: "IX_MiningCoinConfigs_PoolId",
                table: "MiningCoinConfigs",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_MiningCoinConfigs_WalletId",
                table: "MiningCoinConfigs",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_MiningDevices_OwnerId",
                table: "MiningDevices",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_MiningDevices_PresetId",
                table: "MiningDevices",
                column: "PresetId");

            migrationBuilder.CreateIndex(
                name: "IX_MiningDevices_RigId",
                table: "MiningDevices",
                column: "RigId");

            migrationBuilder.CreateIndex(
                name: "IX_Pools_CryptocurrencyId",
                table: "Pools",
                column: "CryptocurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Pools_Domain_Port",
                table: "Pools",
                columns: new[] { "Domain", "Port" });

            migrationBuilder.CreateIndex(
                name: "IX_Pools_UserId",
                table: "Pools",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Presets_DeviceName",
                table: "Presets",
                column: "DeviceName");

            migrationBuilder.CreateIndex(
                name: "IX_Presets_Name",
                table: "Presets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Presets_UserId",
                table: "Presets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Address",
                table: "Wallets",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CryptocurrencyId",
                table: "Wallets",
                column: "CryptocurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Name",
                table: "Wallets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MinerAlgorithms");

            migrationBuilder.DropTable(
                name: "MiningCoinConfigs");

            migrationBuilder.DropTable(
                name: "MiningDevices");

            migrationBuilder.DropTable(
                name: "Overclocking");

            migrationBuilder.DropTable(
                name: "Presets");

            migrationBuilder.DropTable(
                name: "FlightSheetTargets");

            migrationBuilder.DropTable(
                name: "Pools");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "GpuOverclocking");

            migrationBuilder.DropTable(
                name: "FlightSheets");

            migrationBuilder.DropTable(
                name: "Miners");

            migrationBuilder.DropTable(
                name: "Cryptocurrencies");

            migrationBuilder.DropTable(
                name: "Algorithms");
        }
    }
}
