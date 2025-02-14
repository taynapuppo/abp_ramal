using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRamais.Migrations
{
    /// <inheritdoc />
    public partial class Add2_Ramal_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedDepartamento",
                table: "AppRamais",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AppRamais",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "AppRamais",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedDepartamento",
                table: "AppRamais");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AppRamais");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "AppRamais");
        }
    }
}
