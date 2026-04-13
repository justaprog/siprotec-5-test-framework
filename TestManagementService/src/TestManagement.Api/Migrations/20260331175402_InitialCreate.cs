using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DeviceFamily = table.Column<string>(type: "text", nullable: false),
                    ProtectionFunction = table.Column<string>(type: "text", nullable: false),
                    FaultType = table.Column<string>(type: "text", nullable: false),
                    PickupCurrent = table.Column<double>(type: "double precision", nullable: false),
                    FaultCurrent = table.Column<double>(type: "double precision", nullable: false),
                    FaultStartMs = table.Column<int>(type: "integer", nullable: false),
                    TripDelayMs = table.Column<int>(type: "integer", nullable: false),
                    ExpectedTrip = table.Column<bool>(type: "boolean", nullable: false),
                    ExpectedTripMinMs = table.Column<int>(type: "integer", nullable: true),
                    ExpectedTripMaxMs = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestCaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualTrip = table.Column<bool>(type: "boolean", nullable: true),
                    ActualTripTimeMs = table.Column<int>(type: "integer", nullable: true),
                    Passed = table.Column<bool>(type: "boolean", nullable: true),
                    ResultMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestRuns_TestCases_TestCaseId",
                        column: x => x.TestCaseId,
                        principalTable: "TestCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestRuns_TestCaseId",
                table: "TestRuns",
                column: "TestCaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestRuns");

            migrationBuilder.DropTable(
                name: "TestCases");
        }
    }
}
