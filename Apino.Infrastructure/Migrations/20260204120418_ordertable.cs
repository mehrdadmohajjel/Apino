using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ordertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BranchUserId",
                table: "OrderStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OrderStatusTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ShowName",
                value: " آماده تحویل ");

            migrationBuilder.InsertData(
                table: "OrderStatusTypes",
                columns: new[] { "Id", "EnglishName", "ShowName" },
                values: new object[] { 8L, "Deliverd", "تحویل داده شده" });

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2913));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2926));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2927));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2928));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2930));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2931));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2933));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2934));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2936));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDateTime",
                value: new DateTime(2026, 2, 4, 15, 34, 17, 717, DateTimeKind.Local).AddTicks(2937));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 4, 12, 4, 17, 717, DateTimeKind.Utc).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 4, 12, 4, 17, 717, DateTimeKind.Utc).AddTicks(3031));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 4, 12, 4, 17, 717, DateTimeKind.Utc).AddTicks(3032));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDatetime",
                value: new DateTime(2026, 2, 4, 12, 4, 17, 717, DateTimeKind.Utc).AddTicks(3033));

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_BranchUserId",
                table: "OrderStatuses",
                column: "BranchUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatuses_BranchUsers_BranchUserId",
                table: "OrderStatuses",
                column: "BranchUserId",
                principalTable: "BranchUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatuses_BranchUsers_BranchUserId",
                table: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatuses_BranchUserId",
                table: "OrderStatuses");

            migrationBuilder.DeleteData(
                table: "OrderStatusTypes",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DropColumn(
                name: "BranchUserId",
                table: "OrderStatuses");

            migrationBuilder.UpdateData(
                table: "OrderStatusTypes",
                keyColumn: "Id",
                keyValue: 5L,
                column: "ShowName",
                value: "تحویل شده");

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
        }
    }
}
