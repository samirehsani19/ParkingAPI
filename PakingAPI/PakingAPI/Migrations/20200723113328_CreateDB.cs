using Microsoft.EntityFrameworkCore.Migrations;

namespace PakingAPI.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    ParkingID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(nullable: false),
                    City = table.Column<string>(maxLength: 25, nullable: false),
                    StreetAdress = table.Column<string>(nullable: false),
                    FreeParkingStart = table.Column<string>(nullable: true),
                    FreeParkingEnd = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parkings", x => x.ParkingID);
                    table.ForeignKey(
                        name: "FK_Parkings_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Rate = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Parkings_ParkingID",
                        column: x => x.ParkingID,
                        principalTable: "Parkings",
                        principalColumn: "ParkingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Age", "Email", "FirstName", "LastName" },
                values: new object[] { 1, 24, "samir20jan@gmail.com", "Samir", "Jan" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Age", "Email", "FirstName", "LastName" },
                values: new object[] { 2, 30, "johan@gmail.com", "Johan", "Berg" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Age", "Email", "FirstName", "LastName" },
                values: new object[] { 3, 20, "david.jahn@gmail.com", "David", "Jahnson" });

            migrationBuilder.InsertData(
                table: "Parkings",
                columns: new[] { "ParkingID", "City", "Country", "FreeParkingEnd", "FreeParkingStart", "StreetAdress", "UserID" },
                values: new object[] { 1, "Gathenburg", "Sweden", "Ends at 15 march", "free 24 h", "Danska vägen", 1 });

            migrationBuilder.InsertData(
                table: "Parkings",
                columns: new[] { "ParkingID", "City", "Country", "FreeParkingEnd", "FreeParkingStart", "StreetAdress", "UserID" },
                values: new object[] { 3, "Uppsala", "Sweden", "Ends at 08:00", "free from 18:00", "Gränby vägen", 1 });

            migrationBuilder.InsertData(
                table: "Parkings",
                columns: new[] { "ParkingID", "City", "Country", "FreeParkingEnd", "FreeParkingStart", "StreetAdress", "UserID" },
                values: new object[] { 2, "Gathenburg", "Sweden", "Ends at 08:00", "free from 18:00", "Exportgatan", 3 });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackID", "Comment", "ParkingID", "Rate", "UserID" },
                values: new object[] { 1, "Greate parking", 1, 8, 2 });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackID", "Comment", "ParkingID", "Rate", "UserID" },
                values: new object[] { 2, "There is no parking at this adress", 2, 1, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ParkingID",
                table: "Feedbacks",
                column: "ParkingID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserID",
                table: "Feedbacks",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_UserID",
                table: "Parkings",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Parkings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
