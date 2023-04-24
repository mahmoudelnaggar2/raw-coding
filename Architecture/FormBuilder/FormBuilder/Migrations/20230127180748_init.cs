using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FormBuilder.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSpecs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldSpecs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false),
                    Hint = table.Column<string>(type: "text", nullable: false),
                    InputType = table.Column<int>(type: "integer", nullable: false),
                    Metadata = table.Column<string>(type: "text", nullable: false),
                    FormSpecId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldSpecs_FormSpecs_FormSpecId",
                        column: x => x.FormSpecId,
                        principalTable: "FormSpecs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FormSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    FormSpecId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormSubmissions_FormSpecs_FormSpecId",
                        column: x => x.FormSpecId,
                        principalTable: "FormSpecs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldSpecs_FormSpecId",
                table: "FieldSpecs",
                column: "FormSpecId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_FormSpecId",
                table: "FormSubmissions",
                column: "FormSpecId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldSpecs");

            migrationBuilder.DropTable(
                name: "FormSubmissions");

            migrationBuilder.DropTable(
                name: "FormSpecs");
        }
    }
}
