using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.WebApp.Compartilhado.Infra.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_CK_TBCompromisso_HorarioValido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_TBCompromisso_HorarioValido",
                table: "TBCompromisso",
                sql: "[HoraTermino] > [HoraInicio]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_TBCompromisso_HorarioValido",
                table: "TBCompromisso");
        }
    }
}
