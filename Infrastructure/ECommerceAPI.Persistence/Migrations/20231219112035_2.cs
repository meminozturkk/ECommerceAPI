using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFilesId",
                table: "ProductProductImageFile");

            migrationBuilder.RenameColumn(
                name: "ProductImageFilesId",
                table: "ProductProductImageFile",
                newName: "ProductImageFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFileId",
                table: "ProductProductImageFile",
                column: "ProductImageFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFileId",
                table: "ProductProductImageFile");

            migrationBuilder.RenameColumn(
                name: "ProductImageFileId",
                table: "ProductProductImageFile",
                newName: "ProductImageFilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFilesId",
                table: "ProductProductImageFile",
                column: "ProductImageFilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
