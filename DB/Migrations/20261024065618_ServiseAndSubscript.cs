using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ServiseAndSubscript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BasePrice = table.Column<long>(type: "bigint", nullable: false),
                    DefaultPaymentTerm = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomName = table.Column<string>(type: "text", nullable: true),
                    ActualPrice = table.Column<long>(type: "bigint", nullable: false),
                    Autoprolong = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentTerm = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Service_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                table: "Service",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CatalogId",
                table: "Subscription",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CustomerId",
                table: "Subscription",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Service");
        }
    }
}
