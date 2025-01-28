using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaRamais.Migrations
{
    /// <inheritdoc />
    public partial class Add_Ramal_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "AppRamais");

            migrationBuilder.RenameColumn(
                name: "Responsavel",
                table: "AppRamais",
                newName: "Nome");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AppRamais",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "AppRamais",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE AppRamais
                SET 
                    NormalizedEmail = unaccent(LOWER(Email)),
                    NormalizedName = unaccent(LOWER(Nome))
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AppRamais");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "AppRamais");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "AppRamais",
                newName: "Responsavel");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "AppRamais",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
