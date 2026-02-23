using Microsoft.EntityFrameworkCore.Migrations;

namespace My2Home.Web.Migrations
{
    public partial class userproperties2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
