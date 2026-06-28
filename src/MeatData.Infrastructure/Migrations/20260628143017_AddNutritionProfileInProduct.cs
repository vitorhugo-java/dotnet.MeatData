using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeatData.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNutritionProfileInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NutritionProfiles_ProductId",
                table: "NutritionProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionProfiles_ProductId",
                table: "NutritionProfiles",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NutritionProfiles_ProductId",
                table: "NutritionProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionProfiles_ProductId",
                table: "NutritionProfiles",
                column: "ProductId");
        }
    }
}
