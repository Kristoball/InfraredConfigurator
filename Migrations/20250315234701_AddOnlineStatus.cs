using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraredConfigurator.Migrations
{
    /// <inheritdoc />
    public partial class AddOnlineStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OnlineStatus",
                table: "ProxyConfigs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlineStatus",
                table: "ProxyConfigs");
        }
    }
}
