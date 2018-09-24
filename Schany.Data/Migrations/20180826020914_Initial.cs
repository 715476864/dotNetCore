using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Schany.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 18, nullable: false),
                    TrueName = table.Column<string>(maxLength: 50, nullable: false),
                    Pic = table.Column<string>(maxLength: 200, nullable: true),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    LoginErrorTimes = table.Column<int>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyDictionaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    DicType = table.Column<int>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 20, nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(nullable: true),
                    LastUpdatedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyDictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyDictionaries_Customers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyDictionaries_CreateUserId",
                table: "MyDictionaries",
                column: "CreateUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyDictionaries");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
