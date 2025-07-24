using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionHotel.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSignalementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Signalements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChambreId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DateSignalement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Traite = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signalements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signalements_Chambres_ChambreId",
                        column: x => x.ChambreId,
                        principalTable: "Chambres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Signalements_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Signalements_ChambreId",
                table: "Signalements",
                column: "ChambreId");

            migrationBuilder.CreateIndex(
                name: "IX_Signalements_ReservationId",
                table: "Signalements",
                column: "ReservationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Signalements");
        }
    }
}
