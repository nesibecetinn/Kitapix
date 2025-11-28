using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kitapix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class bookchapterupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MongoDbId",
                table: "BookChapters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MongoDbId",
                table: "BookChapters");
        }
    }
}
