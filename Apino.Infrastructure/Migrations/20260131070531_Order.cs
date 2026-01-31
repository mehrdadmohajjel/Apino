using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentTransactionCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6224));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6225));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6227));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6228));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6229));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6230));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6231));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6233));

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreationDateTime",
                value: new DateTime(2026, 1, 31, 10, 35, 31, 576, DateTimeKind.Local).AddTicks(6259));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 7, 5, 31, 576, DateTimeKind.Utc).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 7, 5, 31, 576, DateTimeKind.Utc).AddTicks(6323));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 7, 5, 31, 576, DateTimeKind.Utc).AddTicks(6324));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 7, 5, 31, 576, DateTimeKind.Utc).AddTicks(6325));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentTransactionCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
