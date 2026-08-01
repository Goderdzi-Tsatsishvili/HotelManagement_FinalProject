using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationRooms_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_ReservationRooms_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationRoomReservationId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ReservationRoomRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ReservationRoomReservationId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationRoomRoomId",
                table: "Reservations");

            migrationBuilder.CreateTable(
                name: "ReservationReservationRoom",
                columns: table => new
                {
                    ReservationsId = table.Column<int>(type: "int", nullable: false),
                    ReservationRoomsReservationId = table.Column<int>(type: "int", nullable: false),
                    ReservationRoomsRoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationReservationRoom", x => new { x.ReservationsId, x.ReservationRoomsReservationId, x.ReservationRoomsRoomId });
                    table.ForeignKey(
                        name: "FK_ReservationReservationRoom_ReservationRooms_ReservationRoomsReservationId_ReservationRoomsRoomId",
                        columns: x => new { x.ReservationRoomsReservationId, x.ReservationRoomsRoomId },
                        principalTable: "ReservationRooms",
                        principalColumns: new[] { "ReservationId", "RoomId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationReservationRoom_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRoomRoom",
                columns: table => new
                {
                    RoomsId = table.Column<int>(type: "int", nullable: false),
                    ReservationRoomsReservationId = table.Column<int>(type: "int", nullable: false),
                    ReservationRoomsRoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRoomRoom", x => new { x.RoomsId, x.ReservationRoomsReservationId, x.ReservationRoomsRoomId });
                    table.ForeignKey(
                        name: "FK_ReservationRoomRoom_ReservationRooms_ReservationRoomsReservationId_ReservationRoomsRoomId",
                        columns: x => new { x.ReservationRoomsReservationId, x.ReservationRoomsRoomId },
                        principalTable: "ReservationRooms",
                        principalColumns: new[] { "ReservationId", "RoomId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationRoomRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationReservationRoom_ReservationRoomsReservationId_ReservationRoomsRoomId",
                table: "ReservationReservationRoom",
                columns: new[] { "ReservationRoomsReservationId", "ReservationRoomsRoomId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRoomRoom_ReservationRoomsReservationId_ReservationRoomsRoomId",
                table: "ReservationRoomRoom",
                columns: new[] { "ReservationRoomsReservationId", "ReservationRoomsRoomId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationReservationRoom");

            migrationBuilder.DropTable(
                name: "ReservationRoomRoom");

            migrationBuilder.AddColumn<int>(
                name: "ReservationRoomReservationId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationRoomRoomId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationRoomReservationId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationRoomRoomId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Rooms",
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Reservations",
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationRooms_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Reservations",
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" },
                principalTable: "ReservationRooms",
                principalColumns: new[] { "ReservationId", "RoomId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_ReservationRooms_ReservationRoomReservationId_ReservationRoomRoomId",
                table: "Rooms",
                columns: new[] { "ReservationRoomReservationId", "ReservationRoomRoomId" },
                principalTable: "ReservationRooms",
                principalColumns: new[] { "ReservationId", "RoomId" });
        }
    }
}
