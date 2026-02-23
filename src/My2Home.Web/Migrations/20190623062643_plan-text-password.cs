using Microsoft.EntityFrameworkCore.Migrations;

namespace My2Home.Web.Migrations
{
    public partial class plantextpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlanTextPassword",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanTextPassword",
                table: "AspNetUsers");
        }
    }
}
