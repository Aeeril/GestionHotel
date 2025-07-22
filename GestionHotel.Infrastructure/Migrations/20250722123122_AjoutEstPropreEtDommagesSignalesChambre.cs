using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionHotel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjoutEstPropreEtDommagesSignalesChambre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstCheckIn",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PaiementEffectue",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DommagesSignales",
                table: "Chambres",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EstPropre",
                table: "Chambres",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstCheckIn",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PaiementEffectue",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "DommagesSignales",
                table: "Chambres");

            migrationBuilder.DropColumn(
                name: "EstPropre",
                table: "Chambres");
        }
    }
}
