using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SS14.ServerHub.Shared.Migrations
{
    /// <inheritdoc />
    public partial class PruneWithTimeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "StartAddressRange",
                table: "TrackedCommunityAddress",
                type: "varbinary(16)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<byte[]>(
                name: "EndAddressRange",
                table: "TrackedCommunityAddress",
                type: "varbinary(16)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "StartAddressRange",
                table: "TrackedCommunityAddress",
                type: "varbinary(16)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<byte[]>(
                name: "EndAddressRange",
                table: "TrackedCommunityAddress",
                type: "varbinary(16)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(16)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }
    }
}
