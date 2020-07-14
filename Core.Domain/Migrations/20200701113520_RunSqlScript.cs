using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace Core.Domain.Migrations
{
    public partial class RunSqlScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sqlText = File.ReadAllText(@"DataSeed.txt");
            migrationBuilder.Sql(sqlText);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
