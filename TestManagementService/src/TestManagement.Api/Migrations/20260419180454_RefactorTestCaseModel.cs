using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTestCaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TripDelayMs",
                table: "TestCases",
                newName: "ExpectedOutcome_TripDelayMs");

            migrationBuilder.RenameColumn(
                name: "PickupCurrent",
                table: "TestCases",
                newName: "Simulation_PickupCurrent");

            migrationBuilder.RenameColumn(
                name: "FaultType",
                table: "TestCases",
                newName: "Simulation_FaultType");

            migrationBuilder.RenameColumn(
                name: "FaultStartMs",
                table: "TestCases",
                newName: "Simulation_FaultStartMs");

            migrationBuilder.RenameColumn(
                name: "FaultCurrent",
                table: "TestCases",
                newName: "Simulation_FaultCurrent");

            migrationBuilder.RenameColumn(
                name: "ExpectedTripMinMs",
                table: "TestCases",
                newName: "ExpectedOutcome_ExpectedTripMinMs");

            migrationBuilder.RenameColumn(
                name: "ExpectedTripMaxMs",
                table: "TestCases",
                newName: "ExpectedOutcome_ExpectedTripMaxMs");

            migrationBuilder.RenameColumn(
                name: "ExpectedTrip",
                table: "TestCases",
                newName: "ExpectedOutcome_ExpectedTrip");

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedOutcome_TripDelayMs",
                table: "TestCases",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Simulation_DurationMs",
                table: "TestCases",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Simulation_NominalCurrent",
                table: "TestCases",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Simulation_SamplingRateHz",
                table: "TestCases",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Simulation_DurationMs",
                table: "TestCases");

            migrationBuilder.DropColumn(
                name: "Simulation_NominalCurrent",
                table: "TestCases");

            migrationBuilder.DropColumn(
                name: "Simulation_SamplingRateHz",
                table: "TestCases");

            migrationBuilder.RenameColumn(
                name: "Simulation_PickupCurrent",
                table: "TestCases",
                newName: "PickupCurrent");

            migrationBuilder.RenameColumn(
                name: "Simulation_FaultType",
                table: "TestCases",
                newName: "FaultType");

            migrationBuilder.RenameColumn(
                name: "Simulation_FaultStartMs",
                table: "TestCases",
                newName: "FaultStartMs");

            migrationBuilder.RenameColumn(
                name: "Simulation_FaultCurrent",
                table: "TestCases",
                newName: "FaultCurrent");

            migrationBuilder.RenameColumn(
                name: "ExpectedOutcome_TripDelayMs",
                table: "TestCases",
                newName: "TripDelayMs");

            migrationBuilder.RenameColumn(
                name: "ExpectedOutcome_ExpectedTripMinMs",
                table: "TestCases",
                newName: "ExpectedTripMinMs");

            migrationBuilder.RenameColumn(
                name: "ExpectedOutcome_ExpectedTripMaxMs",
                table: "TestCases",
                newName: "ExpectedTripMaxMs");

            migrationBuilder.RenameColumn(
                name: "ExpectedOutcome_ExpectedTrip",
                table: "TestCases",
                newName: "ExpectedTrip");

            migrationBuilder.AlterColumn<int>(
                name: "TripDelayMs",
                table: "TestCases",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
