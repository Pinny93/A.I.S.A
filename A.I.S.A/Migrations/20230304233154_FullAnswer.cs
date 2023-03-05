using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A.I.S.A_.Migrations
{
    /// <inheritdoc />
    public partial class FullAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnswerFull",
                table: "Queries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerFull",
                table: "Queries");
        }
    }
}