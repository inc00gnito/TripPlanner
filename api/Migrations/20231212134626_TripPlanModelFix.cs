using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class TripPlanModelFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPlaces_TripPlan_TripPlanId",
                table: "TripPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripPlan",
                table: "TripPlan");

            migrationBuilder.RenameTable(
                name: "TripPlan",
                newName: "TripPlans");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "TripPlans",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripPlans",
                table: "TripPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPlaces_TripPlans_TripPlanId",
                table: "TripPlaces",
                column: "TripPlanId",
                principalTable: "TripPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPlaces_TripPlans_TripPlanId",
                table: "TripPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripPlans",
                table: "TripPlans");

            migrationBuilder.RenameTable(
                name: "TripPlans",
                newName: "TripPlan");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "TripPlan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripPlan",
                table: "TripPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPlaces_TripPlan_TripPlanId",
                table: "TripPlaces",
                column: "TripPlanId",
                principalTable: "TripPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
