using CatechistHelper.Infrastructure.Utils;


namespace CatechistHelper.Testing
{
    public class HandleExcelTest
    {
        [Fact]
        public void ReadFile_ShouldExtractDataCorrectly()
        {
            // Arrange
            var filePath = "C:\\Users\\ADMIN\\Downloads\\class_data.xlsx"; // Path to a test Excel file

            var file = FileHelper.ReadFileToIFormFile(filePath);
            // Act
            var result = FileHelper.ReadFile(file);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Name);
            Assert.NotEmpty(result.Majors);
        }

    }

    
}
