using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Schany.Data.Migrations
{
    public partial class Add_CustomerOpenPlateAccount_Module_PresetDataPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerOpenPlateAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    OpenPlateType = table.Column<int>(nullable: false),
                    Account = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOpenPlateAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOpenPlateAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Text = table.Column<string>(maxLength: 50, nullable: true),
                    DisplayNo = table.Column<int>(nullable: false),
                    AppendClass = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 100, nullable: true),
                    ParentModuleId = table.Column<Guid>(nullable: true),
                    HasPermission = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Modules_ParentModuleId",
                        column: x => x.ParentModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresetDataPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Text = table.Column<string>(maxLength: 50, nullable: true),
                    ModuleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresetDataPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresetDataPermissions_Customers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresetDataPermissions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOpenPlateAccounts_CustomerId",
                table: "CustomerOpenPlateAccounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ParentModuleId",
                table: "Modules",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PresetDataPermissions_CreateUserId",
                table: "PresetDataPermissions",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PresetDataPermissions_ModuleId",
                table: "PresetDataPermissions",
                column: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerOpenPlateAccounts");

            migrationBuilder.DropTable(
                name: "PresetDataPermissions");

            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
