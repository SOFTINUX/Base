using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class NewPermissionsSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModulesForUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    AllowedPaidForModules = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulesForUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RolesToPermissions",
                columns: table => new
                {
                    RoleName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    PermissionsInRole = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesToPermissions", x => x.RoleName);
                });

            migrationBuilder.CreateTable(
                name: "TimeStores",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 36, nullable: false),
                    LastUpdatedTicks = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeStores", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "UserToRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    RoleName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToRoles", x => new { x.UserId, x.RoleName });
                    table.ForeignKey(
                        name: "FK_UserToRoles_RolesToPermissions_RoleName",
                        column: x => x.RoleName,
                        principalTable: "RolesToPermissions",
                        principalColumn: "RoleName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToRoles_RoleName",
                table: "UserToRoles",
                column: "RoleName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModulesForUsers");

            migrationBuilder.DropTable(
                name: "TimeStores");

            migrationBuilder.DropTable(
                name: "UserToRoles");

            migrationBuilder.DropTable(
                name: "RolesToPermissions");
        }
    }
}
