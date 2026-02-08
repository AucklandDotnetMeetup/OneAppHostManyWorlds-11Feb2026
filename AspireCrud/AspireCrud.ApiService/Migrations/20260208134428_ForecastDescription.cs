using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspireCrud.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ForecastDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WeatherForecasts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WeatherForecasts");
        }
    }
}
