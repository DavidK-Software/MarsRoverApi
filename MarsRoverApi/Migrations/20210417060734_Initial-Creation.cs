using Microsoft.EntityFrameworkCore.Migrations;

namespace MarsRoverApi.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rovers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    LandingDate = table.Column<string>(type: "TEXT", nullable: true),
                    LaunchDate = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rovers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RoverId = table.Column<int>(type: "INTEGER", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_Rovers_RoverId",
                        column: x => x.RoverId,
                        principalTable: "Rovers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarsPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NasaPhotoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Sol = table.Column<int>(type: "INTEGER", nullable: false),
                    ImgSrc = table.Column<string>(type: "TEXT", nullable: true),
                    EarthDate = table.Column<string>(type: "TEXT", nullable: true),
                    CameraId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoverId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarsPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarsPhotos_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarsPhotos_Rovers_RoverId",
                        column: x => x.RoverId,
                        principalTable: "Rovers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_RoverId",
                table: "Cameras",
                column: "RoverId");

            migrationBuilder.CreateIndex(
                name: "IX_MarsPhotos_CameraId",
                table: "MarsPhotos",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_MarsPhotos_RoverId",
                table: "MarsPhotos",
                column: "RoverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarsPhotos");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "Rovers");
        }
    }
}
