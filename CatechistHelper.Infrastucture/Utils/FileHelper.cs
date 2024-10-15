using CatechistHelper.Domain.Dtos.Responses.Timetable;
using CatechistHelper.Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;


namespace CatechistHelper.Infrastructure.Utils
{
    public static class FileHelper
    {
        public static PastoralYearDto ReadFile(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var tempFilePath = Path.GetRandomFileName();

            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            var fileInfo = new FileInfo(tempFilePath);
            return ReadFile(fileInfo);
        }

        public static PastoralYearDto ReadFile(FileInfo file)
        {
            var pastoralYear = new PastoralYearDto();

            using (var package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                pastoralYear.Name = worksheet.Cells[2, 5].Text;

                for (int row = 2; row <= rowCount; row++)
                {
                    var className = worksheet.Cells[row, 1].Text;
                    var gradeName = worksheet.Cells[row, 2].Text;
                    var majorName = worksheet.Cells[row, 3].Text;
                    var numberOfCatechist = int.Parse(worksheet.Cells[row, 4].Text);

                    var classDto = new ClassDto
                    {
                        Name = className,
                        NumberOfCatechist = numberOfCatechist
                    };

                    var majorDto = pastoralYear.Majors.FirstOrDefault(m => m.Name == majorName);
                    if (majorDto == null)
                    {
                        majorDto = new MajorDto
                        {
                            Name = majorName,
                            Grades = []
                        };
                        pastoralYear.Majors.Add(majorDto);
                    }

                    var gradeDto = majorDto.Grades.FirstOrDefault(g => g.Name == gradeName);
                    if (gradeDto == null)
                    {
                        gradeDto = new GradeDto
                        {
                            Name = gradeName,
                            Classes = []
                        };
                        majorDto.Grades.Add(gradeDto);
                    }

                    gradeDto.Classes.Add(classDto);
                }
            }
            return pastoralYear;
        }

        public static IFormFile ReadFileToIFormFile(string filePath)
        {
            try
            {
                // Read the file into a byte array
                byte[] fileBytes = File.ReadAllBytes(filePath);

                // Create a memory stream from the byte array
                var memoryStream = new MemoryStream(fileBytes);

                // Extract the file name from the file path
                var fileName = Path.GetFileName(filePath);

                // Create an IFormFile object from the memory stream
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream" // Adjust the content type if necessary
                };

                return formFile;
            }
            catch (Exception ex)
            {
                // Log the exception and throw if needed
                Console.WriteLine($"Error reading file to IFormFile: {ex.Message}");
                throw;
            }
        }

        public static byte[] ExportToExcel(Class classDto)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Slots");

            // Set the headers
            worksheet.Cells[1, 1].Value = "SlotId";
            worksheet.Cells[1, 2].Value = "StartTime";
            worksheet.Cells[1, 3].Value = "EndTime";
            worksheet.Cells[1, 4].Value = "Date";
            worksheet.Cells[1, 5].Value = "Note";
            worksheet.Cells[1, 6].Value = "RoomName";
            worksheet.Cells[1, 7].Value = "ClassName";
            worksheet.Cells[1, 8].Value = "Catechists";

            int row = 2;
            foreach (var slot in classDto.Slots)
            {
                worksheet.Cells[row, 1].Value = slot.Id;
                worksheet.Cells[row, 2].Value = slot.StartTime.ToString("HH:mm");
                worksheet.Cells[row, 3].Value = slot.EndTime.ToString("HH:mm");
                worksheet.Cells[row, 4].Value = slot.Date.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 5].Value = slot.Note ?? string.Empty;
                worksheet.Cells[row, 6].Value = slot.Room.Name;
                worksheet.Cells[row, 7].Value = slot.Class.Name;

                // Join catechist names into a single string
                var catechists = string.Join(", ", slot.CatechistInSlots.Select(c => c.Catechist.Account.FullName));
                worksheet.Cells[row, 8].Value = catechists;

                row++;
            }
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}
