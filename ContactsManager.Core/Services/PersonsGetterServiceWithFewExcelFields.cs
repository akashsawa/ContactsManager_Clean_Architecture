using Entities;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
    {
        private readonly PersonsGetterService _personsGetterService;

        public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
        {
            _personsGetterService = personsGetterService;
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            return await _personsGetterService.GetAllPersons();
        }

        public async Task<List<PersonResponse>> GetFilteredpersons(string searchby, string? searchString)
        {
            return await _personsGetterService.GetFilteredpersons(searchby, searchString);
        }

        public async Task<PersonResponse>? GetpersonByPersonId(Guid? personid)
        {
            return await _personsGetterService.GetpersonByPersonId(personid);
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            return await _personsGetterService.GetPersonsCSV();
        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream(); //it can contain image/excel/csv i.e. any type of data of file.

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date Of Birth";

                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";


                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;

                

                //List<PersonResponse> persons = _personRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();

                List<PersonResponse> persons = await GetAllPersons();

                //FOR LISKOV PRINCIPLE
                if (persons.Count == 0)
                    throw new InvalidOperationException("persons count cannot be 0 !");
                //FOR LISKOV PRINCIPLE

                foreach (PersonResponse personResponse in persons)
                {
                    worksheet.Cells[row, 1].Value = personResponse.PersonName;
                    worksheet.Cells[row, 2].Value = personResponse.Email;
                    if (personResponse.DateOfBirth.HasValue)
                    {
                        worksheet.Cells[row, 3].Value = personResponse.DateOfBirth.Value.ToString("yyyy-MM-dd");
                    }
                    else
                        worksheet.Cells[row, 3].Value = "";
                    worksheet.Cells[row, 3].Value = personResponse.DateOfBirth;

                    worksheet.Cells[row, 6].Value = personResponse.Country;
                    worksheet.Cells[row, 7].Value = personResponse.Address;


                    row++;
                }

                //for column width:
                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}

