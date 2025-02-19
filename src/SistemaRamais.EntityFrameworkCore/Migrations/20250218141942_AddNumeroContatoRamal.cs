using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRamais.Migrations
{
    /// <inheritdoc />
    public partial class AddNumeroContatoRamal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "AppRamais");

            migrationBuilder.AddColumn<string>(
                name: "Numero_Celular",
                table: "AppRamais",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Numero_Ramal",
                table: "AppRamais",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero_Celular",
                table: "AppRamais");

            migrationBuilder.DropColumn(
                name: "Numero_Ramal",
                table: "AppRamais");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "AppRamais",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
