using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfraredConfigurator.Migrations
{
    /// <inheritdoc />
    public partial class AddProxyConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProxyConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Port = table.Column<string>(type: "TEXT", nullable: false),
                    SubDomain = table.Column<string>(type: "TEXT", nullable: false),
                    DomainId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisconnectMessage = table.Column<string>(type: "TEXT", nullable: false),
                    OfflineStatus = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProxyConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProxyConfigs_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProxyConfigs_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProxyConfigs_DomainId",
                table: "ProxyConfigs",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_ProxyConfigs_ServerId",
                table: "ProxyConfigs",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProxyConfigs");
        }
    }
}
