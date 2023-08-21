using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SS14.ServerHub.Shared.Migrations
{
    /// <inheritdoc />
    public partial class MySQLSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdvertisedServer",
                columns: table => new
                {
                    AdvertisedServerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expires = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StatusData = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InfoData = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdvertiserAddress = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisedServer", x => x.AdvertisedServerId);
                    table.CheckConstraint("AddressSs14Uri", "`Address` LIKE 'ss14://%' OR `Address` LIKE 'ss14s://%'");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HubAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Actor = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubAudit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackedCommunity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsBanned = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedCommunity", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServerStatusArchive",
                columns: table => new
                {
                    AdvertisedServerId = table.Column<int>(type: "int", nullable: false),
                    ServerStatusArchiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StatusData = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdvertiserAddress = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerStatusArchive", x => new { x.AdvertisedServerId, x.ServerStatusArchiveId });
                    table.ForeignKey(
                        name: "FK_ServerStatusArchive_AdvertisedServer_AdvertisedServerId",
                        column: x => x.AdvertisedServerId,
                        principalTable: "AdvertisedServer",
                        principalColumn: "AdvertisedServerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UniqueServerName",
                columns: table => new
                {
                    AdvertisedServerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstSeen = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueServerName", x => new { x.AdvertisedServerId, x.Name });
                    table.ForeignKey(
                        name: "FK_UniqueServerName_AdvertisedServer_AdvertisedServerId",
                        column: x => x.AdvertisedServerId,
                        principalTable: "AdvertisedServer",
                        principalColumn: "AdvertisedServerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackedCommunityAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrackedCommunityId = table.Column<int>(type: "int", nullable: false),
                    EndAddressRange = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    StartAddressRange = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedCommunityAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedCommunityAddress_TrackedCommunity_TrackedCommunityId",
                        column: x => x.TrackedCommunityId,
                        principalTable: "TrackedCommunity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackedCommunityDomain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DomainName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrackedCommunityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedCommunityDomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedCommunityDomain_TrackedCommunity_TrackedCommunityId",
                        column: x => x.TrackedCommunityId,
                        principalTable: "TrackedCommunity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisedServer_Address",
                table: "AdvertisedServer",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HubAudit_Time",
                table: "HubAudit",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedCommunityAddress_EndAddressRange",
                table: "TrackedCommunityAddress",
                column: "EndAddressRange");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedCommunityAddress_StartAddressRange",
                table: "TrackedCommunityAddress",
                column: "StartAddressRange");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedCommunityAddress_TrackedCommunityId",
                table: "TrackedCommunityAddress",
                column: "TrackedCommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedCommunityDomain_TrackedCommunityId",
                table: "TrackedCommunityDomain",
                column: "TrackedCommunityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubAudit");

            migrationBuilder.DropTable(
                name: "ServerStatusArchive");

            migrationBuilder.DropTable(
                name: "TrackedCommunityAddress");

            migrationBuilder.DropTable(
                name: "TrackedCommunityDomain");

            migrationBuilder.DropTable(
                name: "UniqueServerName");

            migrationBuilder.DropTable(
                name: "TrackedCommunity");

            migrationBuilder.DropTable(
                name: "AdvertisedServer");
        }
    }
}
