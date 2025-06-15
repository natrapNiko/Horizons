﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Horizons.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "7699db7d-964f-4782-8209-d76562e0fece", 0, "a2d2c999-cfcc-448f-8b8d-e83bf8056df8", "admin@horizons.com", true, false, null, "ADMIN@HORIZONS.COM", "ADMIN@HORIZONS.COM", "AQAAAAIAAYagAAAAEFJ9f5/YXEunz7vxcIDGLt4HdisWctWbV6WWEv0K92jYdesYrvTHIy3YcYxpxapbkw==", null, false, "fc65ba77-6448-44d5-b0e6-e4dc54e4fdc4", false, "admin@horizons.com" });

            migrationBuilder.InsertData(
                table: "Terrains",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Mountain" },
                    { 2, "Beach" },
                    { 3, "Forest" },
                    { 4, "Plain" },
                    { 5, "Urban" },
                    { 6, "Village" },
                    { 7, "Cave" },
                    { 8, "Canyon" }
                });

            migrationBuilder.InsertData(
                table: "Destinations",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "PublishedOn", "PublisherId", "TerrainId" },
                values: new object[,]
                {
                    { 1, "A stunning historical landmark nestled in the Rila Mountains.", "https://img.etimg.com/thumb/msid-112831459,width-640,height-480,imgsize-2180890,resizemode-4/rila-monastery-bulgaria.jpg", "Rila Monastery", new DateTime(2025, 6, 15, 14, 32, 25, 975, DateTimeKind.Local).AddTicks(375), "7699db7d-964f-4782-8209-d76562e0fece", 1 },
                    { 2, "The sand at Durankulak Beach showcases a pristine golden color, creating a beautiful contrast against the azure waters of the Black Sea.", "https://travelplanner.ro/blog/wp-content/uploads/2023/01/durankulak-beach-1-850x550.jpg.webp", "Durankulak Beach", new DateTime(2025, 6, 15, 14, 32, 25, 975, DateTimeKind.Local).AddTicks(448), "7699db7d-964f-4782-8209-d76562e0fece", 2 },
                    { 3, "A mysterious cave located in the Rhodope Mountains.", "https://detskotobnr.binar.bg/wp-content/uploads/2017/11/Diavolsko_garlo_17.jpg", "Devil's Throat Cave", new DateTime(2025, 6, 15, 14, 32, 25, 975, DateTimeKind.Local).AddTicks(452), "7699db7d-964f-4782-8209-d76562e0fece", 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece");

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Terrains",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
