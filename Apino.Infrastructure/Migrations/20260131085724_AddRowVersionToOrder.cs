using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Orders",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

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
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3192));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3193));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3194));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreationDatetime",
                value: new DateTime(2026, 1, 31, 8, 57, 23, 748, DateTimeKind.Utc).AddTicks(3195));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Orders");

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
    }
}
