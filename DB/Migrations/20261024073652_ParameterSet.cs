using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class ParameterSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Service_CatalogId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_CatalogId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                table: "Subscription");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Subscription",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Subscription",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Service",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "ParameterSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PSetName = table.Column<string>(type: "text", nullable: false),
                    WeightNoAutoprolong = table.Column<double>(type: "double precision", nullable: false),
                    WeightLowBalance = table.Column<double>(type: "double precision", nullable: false),
                    WeightExpiringSoon = table.Column<double>(type: "double precision", nullable: false),
                    WeightProlongWaitStatus = table.Column<double>(type: "double precision", nullable: false),
                    Risk = table.Column<double>(type: "double precision", nullable: false),
                    FinancialLossHorizonMonths = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterSet", x => x.Id);
                    table.UniqueConstraint("AK_ParameterSet_PSetName", x => x.PSetName);
                    table.CheckConstraint("PS_WeightExpiringSoon", "\"WeightExpiringSoon\" >= 0.0 AND \"WeightExpiringSoon\" <= 1.0");
                    table.CheckConstraint("PS_WeightLowBalance", "\"WeightLowBalance\" >= 0.0 AND \"WeightLowBalance\" <= 1.0");
                    table.CheckConstraint("PS_WeightNoAutoprolong", "\"WeightNoAutoprolong\" >= 0.0 AND \"WeightNoAutoprolong\" <= 1.0");
                    table.CheckConstraint("PS_WeightProlongWaitStatus", "\"WeightProlongWaitStatus\" >= 0.0 AND \"WeightProlongWaitStatus\" <= 1.0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ServiceId",
                table: "Subscription",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Service_ServiceId",
                table: "Subscription",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Service_ServiceId",
                table: "Subscription");

            migrationBuilder.DropTable(
                name: "ParameterSet");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_ServiceId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Subscription");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Subscription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "CatalogId",
                table: "Subscription",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Service",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_CatalogId",
                table: "Subscription",
                column: "CatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Service_CatalogId",
                table: "Subscription",
                column: "CatalogId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
