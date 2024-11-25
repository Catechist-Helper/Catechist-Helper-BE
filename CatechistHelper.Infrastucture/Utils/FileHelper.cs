using CatechistHelper.Domain.Dtos.Responses.Timetable;
using CatechistHelper.Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Globalization;

namespace CatechistHelper.Infrastructure.Utils
{
    public static class FileHelper
    {
        public const string AllowedFormats = "dd/MM/yyyy";

        public static PastoralYearDto ReadFile(IFormFile file)
        {
            CheckXLSXFile(file);

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


        public static byte[] ExportPastoralYearToExcel(PastoralYear year)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();

            foreach (var classDto in year.Classes)
            {
                // Create a worksheet for each class
                var worksheet = package.Workbook.Worksheets.Add(classDto.Name);

                // Set the headers
                worksheet.Cells[1, 1].Value = "SlotId";
                worksheet.Cells[1, 2].Value = "StartTime";
                worksheet.Cells[1, 3].Value = "EndTime";
                worksheet.Cells[1, 4].Value = "Date";
                worksheet.Cells[1, 5].Value = "Note";
                worksheet.Cells[1, 6].Value = "RoomName";
                worksheet.Cells[1, 7].Value = "ClassName";
                worksheet.Cells[1, 8].Value = "Catechists";
                worksheet.Cells[1, 9].Value = "Grade";
                worksheet.Cells[1, 10].Value = "Year";

                int row = 2;
                foreach (var slot in classDto.Slots)
                {
                    worksheet.Cells[row, 1].Value = slot.Id;
                    worksheet.Cells[row, 2].Value = slot.StartTime.ToString("HH:mm");
                    worksheet.Cells[row, 3].Value = slot.EndTime.ToString("HH:mm");
                    worksheet.Cells[row, 4].Value = slot.Date.ToString("dd-MM-yyyy");
                    worksheet.Cells[row, 5].Value = slot.Note ?? string.Empty;
                    worksheet.Cells[row, 6].Value = slot.Room.Name;
                    worksheet.Cells[row, 7].Value = classDto.Name;

                    var catechists = string.Join(", ",
                         slot.CatechistInSlots.Select(c =>
                             c.Catechist.FullName
                         )
                     );

                    worksheet.Cells[row, 8].Value = catechists;
                    worksheet.Cells[row, 9].Value = classDto.Grade.Name;
                    worksheet.Cells[row, 10].Value = year.Name;
                    row++;
                }
                worksheet.Cells.AutoFitColumns();
            }

            return package.GetAsByteArray();
        }


        public static byte[] ExportCatechist(ICollection<Catechist> catechists)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Catechists");

            // Set the headers
            worksheet.Cells[1, 1].Value = "Code";
            worksheet.Cells[1, 2].Value = "Full Name";
            worksheet.Cells[1, 3].Value = "Gender";
            worksheet.Cells[1, 4].Value = "Date of Birth";
            worksheet.Cells[1, 5].Value = "Birth Place";
            worksheet.Cells[1, 6].Value = "Father Name";
            worksheet.Cells[1, 7].Value = "Father Phone";
            worksheet.Cells[1, 8].Value = "Mother Name";
            worksheet.Cells[1, 9].Value = "Mother Phone";
            worksheet.Cells[1, 10].Value = "Image URL";
            worksheet.Cells[1, 11].Value = "Address";
            worksheet.Cells[1, 12].Value = "Phone";
            worksheet.Cells[1, 13].Value = "Level";
            worksheet.Cells[1, 14].Value = "Email";
            int row = 2;
            foreach (var catechist in catechists)
            {
                worksheet.Cells[row, 1].Value = catechist.Code;
                worksheet.Cells[row, 2].Value = catechist.FullName;
                worksheet.Cells[row, 3].Value = catechist.Gender;
                worksheet.Cells[row, 4].Value = catechist.DateOfBirth.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 5].Value = catechist.BirthPlace ?? string.Empty;
                worksheet.Cells[row, 6].Value = catechist.FatherName ?? string.Empty;
                worksheet.Cells[row, 7].Value = catechist.FatherPhone ?? string.Empty;
                worksheet.Cells[row, 8].Value = catechist.MotherName ?? string.Empty;
                worksheet.Cells[row, 9].Value = catechist.MotherPhone ?? string.Empty;
                worksheet.Cells[row, 10].Value = catechist.ImageUrl ?? string.Empty;
                worksheet.Cells[row, 11].Value = catechist.Address ?? string.Empty;
                worksheet.Cells[row, 12].Value = catechist.Phone ?? string.Empty;
                worksheet.Cells[row, 13].Value = catechist.Level.Name ?? string.Empty;
                worksheet.Cells[row, 14].Value = catechist.Account.Email ?? string.Empty;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }

        public static List<ParticipantInEvent> ReadFileReturnParticipants(IFormFile file, Guid eventId)
        {
            CheckXLSXFile(file);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var tempFilePath = Path.GetRandomFileName();

            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var fileInfo = new FileInfo(tempFilePath);
            return ReadFileReturnParticipants(fileInfo, eventId);
        }

        public static List<ParticipantInEvent> ReadFileReturnParticipants(FileInfo file, Guid eventId)
        {

            var participants = new List<ParticipantInEvent>();

            using (var package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var fullName = worksheet.Cells[row, 2].Text;
                    var email = worksheet.Cells[row, 3].Text;
                    var phone = worksheet.Cells[row, 4].Text;
                    var gender = worksheet.Cells[row, 5].Text;
                    var dateOfBirthText = worksheet.Cells[row, 6].Text;
                    var address = worksheet.Cells[row, 7].Text;


                    if (!DateTime.TryParseExact(dateOfBirthText, AllowedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth))
                    {
                        throw new FormatException(
                            $"Invalid date format in row {row}, column 5. Value: '{dateOfBirthText}' does not match expected format: {string.Join(", ", AllowedFormats)}.");
                    }


                    var participant = new ParticipantInEvent
                    {
                        FullName = fullName,
                        Email = email,
                        Phone = phone,
                        Gender = gender,
                        DateOfBirth = dateOfBirth,
                        Address = address,
                        IsAttended = true,
                        EventId = eventId
                    };

                    participants.Add(participant);
                }
            }

            return participants;
        }

        public static void CheckXLSXFile(IFormFile file)
        {
            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                 file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                throw new Exception("Invalid file format. Please upload a valid .xlsx file.");
            }
        }


        public static byte[] ExportParticipantsToExcel(ICollection<ParticipantInEvent> participants)
        {

            // Create an Excel package
            using var package = new ExcelPackage();
            // Add a worksheet
            var worksheet = package.Workbook.Worksheets.Add("Participants");

            // Define the header row
            worksheet.Cells[1, 1].Value = "Full Name";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "Phone";
            worksheet.Cells[1, 4].Value = "Gender";
            worksheet.Cells[1, 5].Value = "Date of Birth";
            worksheet.Cells[1, 6].Value = "Address";

            // Style the header
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }

            // Populate the data
            int row = 2; // Start from the second row (below header)
            foreach (var participant in participants)
            {
                worksheet.Cells[row, 1].Value = participant.FullName;
                worksheet.Cells[row, 2].Value = participant.Email;
                worksheet.Cells[row, 3].Value = participant.Phone;
                worksheet.Cells[row, 4].Value = participant.Gender;
                worksheet.Cells[row, 5].Value = participant.DateOfBirth.ToString(AllowedFormats);
                worksheet.Cells[row, 6].Value = participant.Address;

                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Return the Excel file as a byte array
            return package.GetAsByteArray();
        }

    }
}
