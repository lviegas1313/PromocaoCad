using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroAPI.Migrations
{
    /// <inheritdoc />
    public partial class notas_imagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.CreateTable(
                name: "Imagens",
                columns: table => new
                {
                    NotaFiscalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Dados = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagens", x => x.NotaFiscalId);
                });

            migrationBuilder.CreateTable(
                name: "NumerosSorte",
                columns: table => new
                {
                    Numero = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotaFiscalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSorteio = table.Column<DateOnly>(type: "date", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumerosSorte", x => x.Numero);
                });

            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    NotaCupom = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagemNotaFiscalId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.NotaCupom);
                    table.ForeignKey(
                        name: "FK_NotasFiscais_Imagens_ImagemNotaFiscalId",
                        column: x => x.ImagemNotaFiscalId,
                        principalTable: "Imagens",
                        principalColumn: "NotaFiscalId");
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotaFiscalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Versao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_NotasFiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "NotasFiscais",
                        principalColumn: "NotaCupom",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_Cnpj",
                table: "NotasFiscais",
                column: "Cnpj");

            migrationBuilder.CreateIndex(
                name: "IX_NotasFiscais_ImagemNotaFiscalId",
                table: "NotasFiscais",
                column: "ImagemNotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_NotaFiscalId",
                table: "Produtos",
                column: "NotaFiscalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumerosSorte");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "NotasFiscais");

            migrationBuilder.DropTable(
                name: "Imagens");

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
