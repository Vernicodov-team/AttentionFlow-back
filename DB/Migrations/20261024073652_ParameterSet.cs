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
            // Only the ParameterSet table belongs to this step. Changes to the former
            // Service/Subscription schema are handled by the following migration, which
            // drops those tables entirely; altering their Guid keys to integer here
            // fails in PostgreSQL without an explicit USING clause.
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParameterSet");
        }
    }
}
