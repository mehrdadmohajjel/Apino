using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BrachUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "BranchUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    StartWorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishWorkDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchUsers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9260));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9271));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9272));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9279));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 2, 15, 17, 22, 109, DateTimeKind.Local).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 2, 11, 47, 22, 109, DateTimeKind.Utc).AddTicks(9362));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 2, 11, 47, 22, 109, DateTimeKind.Utc).AddTicks(9363));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 2, 11, 47, 22, 109, DateTimeKind.Utc).AddTicks(9364));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 2, 11, 47, 22, 109, DateTimeKind.Utc).AddTicks(9365));

            migrationBuilder.CreateIndex(
                name: "IX_BranchUsers_BranchId",
                table: "BranchUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchUsers_UserId",
                table: "BranchUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchUsers");

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3111));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3123));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3124));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3125));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3127));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3128));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3129));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3130));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3131));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 12, 27, 23, 748, DateTimeKind.Local).AddTicks(3133));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "BranchId", "CreationDatetime" },
                values: new object[] { null, new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3192) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "BranchId", "CreationDatetime" },
                values: new object[] { null, new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3193) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "BranchId", "CreationDatetime" },
                values: new object[] { null, new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3194) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "BranchId", "CreationDatetime" },
                values: new object[] { null, new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3195) });
        }
    }
}
