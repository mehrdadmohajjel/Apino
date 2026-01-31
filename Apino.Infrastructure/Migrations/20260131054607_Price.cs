using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9091));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9102));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9103));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9105));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9106));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9107));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9108));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9109));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9111));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 9, 16, 7, 360, DateTimeKind.Local).AddTicks(9112));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 5, 46, 7, 360, DateTimeKind.Utc).AddTicks(9198));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 5, 46, 7, 360, DateTimeKind.Utc).AddTicks(9200));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 5, 46, 7, 360, DateTimeKind.Utc).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 5, 46, 7, 360, DateTimeKind.Utc).AddTicks(9201));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Carts");

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4388));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4403));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4405));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4406));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4408));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4409));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4412));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 0, 7, 6, 449, DateTimeKind.Local).AddTicks(4415));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 30, 20, 37, 6, 449, DateTimeKind.Utc).AddTicks(4508));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 30, 20, 37, 6, 449, DateTimeKind.Utc).AddTicks(4509));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 30, 20, 37, 6, 449, DateTimeKind.Utc).AddTicks(4510));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 30, 20, 37, 6, 449, DateTimeKind.Utc).AddTicks(4511));
        }
    }
}
