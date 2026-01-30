using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Apino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OnlyOnlinePayment = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTypeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceTypeEnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSessionBased = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreationDatetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PayAtPlace = table.Column<bool>(type: "bit", nullable: false),
                    IconName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchStaff",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchStaff_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchStaff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentStatusTypeId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentTransactionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TelegramId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProfileCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceTypeId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    StatusTypeId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatuses_OrderStatusTypes_StatusTypeId",
                        column: x => x.StatusTypeId,
                        principalTable: "OrderStatusTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderStatuses_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PayAtPlace = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "Address", "Description", "ImageUrl", "IsActive", "Slug", "Title" },
                values: new object[,]
                {
                    { 1L, "تبریز - ولیعصر-", "اولین شعبه آپینو", "valiasr.jpg", true, "شعبه-ولیعصر-تبریز", "شعبه ولیعصر" },
                    { 2L, "تبریز - آبرسان-فلکه دانشگاه-", "دومین شعبه آپینو", "bloor-tower.jpg", true, "شعبه-بلور-تبریز", "شعبه برج بلور" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatusTypes",
                columns: new[] { "Id", "EnglishName", "ShowName" },
                values: new object[,]
                {
                    { 1L, "Created", "ثبت شده" },
                    { 2L, "Paid", "پرداخت شده" },
                    { 3L, "Accepted", "تایید شعبه" },
                    { 4L, "Preparing", "در حال آماده‌سازی" },
                    { 5L, "Done", "تحویل شده" },
                    { 6L, "Canceled", "لغو شده" },
                    { 7L, "Draft", "در سبد خرید" }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "IsSessionBased", "ServiceTypeEnglishName", "ServiceTypeTitle" },
                values: new object[,]
                {
                    { 1L, true, "CountByUse", "جلسه ای" },
                    { 2L, false, "Monthly", "ماهانه" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BranchId", "CreationDatetime", "IsActive", "Mobile", "Role" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2026, 1, 29, 23, 0, 37, 755, DateTimeKind.Utc).AddTicks(7926), true, "09143010428", 1 },
                    { 2L, null, new DateTime(2026, 1, 29, 23, 0, 37, 755, DateTimeKind.Utc).AddTicks(7927), true, "09359719229", 2 },
                    { 3L, null, new DateTime(2026, 1, 29, 23, 0, 37, 755, DateTimeKind.Utc).AddTicks(7928), true, "09121234567", 3 },
                    { 4L, null, new DateTime(2026, 1, 29, 23, 0, 37, 755, DateTimeKind.Utc).AddTicks(7929), true, "09127654321", 3 }
                });

            migrationBuilder.InsertData(
                table: "BranchStaff",
                columns: new[] { "Id", "BranchId", "Role", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, 1, 2L },
                    { 2L, 1L, 2, 3L },
                    { 3L, 1L, 3, 4L }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "BranchId", "CategoryTitle", "CreationDateTime", "IconName", "IsActive", "PayAtPlace", "Slug" },
                values: new object[,]
                {
                    { 1L, 1L, "فضای کار اشتراکی", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7819), "place.png", true, true, "فضای-کار-اشتراکی" },
                    { 2L, 1L, "کافه", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7830), "coffe.png", true, true, "کافه-1" },
                    { 3L, 1L, "کافه خوردنی", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7832), "coffe.png", true, true, "کافه-خوردنی-1" },
                    { 4L, 1L, "نوشیدنی سرد", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7833), "coffe.png", true, true, "نوشیدنی-سرد-1" },
                    { 5L, 1L, "فروشگاه", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7834), "shop.png", true, true, "فروشگاه-1" },
                    { 6L, 1L, "میز مطالعه عمومی", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7836), "place.png", true, true, "میز-مطالعه-عمومی-1" },
                    { 7L, 1L, "استودیو", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7837), "studio.png", true, true, "استودیو-1" },
                    { 8L, 1L, "فضای کار اشتراکی", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7839), "place.png", true, true, "فضای-کار-اشتراکی" },
                    { 9L, 2L, "فضای کار اشتراکی", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7840), "palce.png", true, true, "فضای-کار-اشتراکی-2" },
                    { 10L, 2L, "کافه", new DateTime(2026, 1, 30, 2, 30, 37, 755, DateTimeKind.Local).AddTicks(7842), "coffe.png", true, true, "کافه-2" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BranchId", "Description", "ImageName", "IsActive", "Price", "ProductCategoryId", "ServiceTypeId", "Stock", "Title" },
                values: new object[,]
                {
                    { 1L, 1L, "37 گرم عصاره قهوه", "Esperso.jpg", true, 1090000m, 2L, 1L, 20, "اسپرسو (۱۰۰ عربیکا+80/20)" },
                    { 2L, 1L, "37 گرم عصاره قهوه + 220 گرم آب جوش", "Americano.jpg", true, 1090000m, 2L, 1L, 20, "آمریکانو (۱۰۰ عربیکا+80/20)" },
                    { 3L, 1L, "37 گرم عصاره قهوه + شکلات کیندر", "Kinder.jpg", true, 2090000m, 2L, 1L, 30, "اسپرسو کیندر" },
                    { 4L, 1L, "220 گرم قهوه دم آوری شده دستگاهی", "lacP_V0rA2874922.jpg", true, 1290000m, 2L, 1L, 20, "قهوه دمی دستگاهی" },
                    { 5L, 1L, "30 گرم سیروپ وانیل + 37 گرم عصاره قهوه + 220 گرم شیر فوم گرفته شده + یخ", "IceLateVanila.jpg", true, 1590000m, 7L, 1L, 10, "آیس لاته وانیل" },
                    { 6L, 1L, "اویشن + لیمو + عسل + دارچین", "Damnoosh-floo.jpg", true, 1480000m, 7L, 1L, 15, "دمنوش سرماخوردگی" },
                    { 7L, 1L, "زعفران + هل سبز + گل محمدی + دارچین", "Safran-Drink.jpg", true, 1580000m, 7L, 1L, 20, "دمنوش زعفران" },
                    { 8L, 1L, "نعنا، سیروپ بلوکاراسئو، آب پرتقال، آب لیمو، آب انار", "DegarDisi.jpg", true, 1590000m, 7L, 1L, 30, "دگردیسی آپینو" },
                    { 9L, 1L, "عرق بیدمشک + گلاب + زعفران", "bidmeshkdrink.jpg", true, 1080000m, 7L, 1L, 10, "نوشیدنی عرق بیدمشک" },
                    { 10L, 1L, "دو عدد تخم مرغ + نان", "nimroo.jpg", true, 1500000m, 6L, 1L, 10, "نیمرو" },
                    { 11L, 1L, "2 عدد تخم مرغ + 150 گرم سس گوجه + نان", "omlet.jpg", true, 1900000m, 6L, 1L, 10, "املت گوجه" },
                    { 12L, 1L, "سرخ شده / هواپز", "diet-potato.jpg", true, 2000000m, 6L, 1L, 50, "سیب زمینی رژیمی" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchStaff_BranchId",
                table: "BranchStaff",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchStaff_UserId",
                table: "BranchStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_OrderId",
                table: "OrderStatuses",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_StatusTypeId",
                table: "OrderStatuses",
                column: "StatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_BranchId",
                table: "ProductCategories",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ServiceTypeId",
                table: "Products",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchStaff");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "OrderStatusTypes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
