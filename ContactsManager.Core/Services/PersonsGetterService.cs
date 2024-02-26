using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using System.Diagnostics;
using ServiceContracts.Enums;
using System.Transactions;
//using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Hosting;
using SerilogTimings;
using Exceptions;

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
    {
        //ilogger
        private readonly ILogger<PersonsGetterService> _logger;
        //ilogger

        //idiagnosticcontext
        private readonly IDiagnosticContext _diagnosticContext;
        //idiagnosticcontext

        //private readonly List<Person> _persons;
        //private readonly ApplicationDBContext _db;
        private readonly IPersonsRepository _personRepository;
        //private readonly ICountriesService
        //    _countriesService;

        //public PersonsService(bool initialize = true)
        //public PersonsService(ApplicationDBContext personDbContext, ICountriesService countriesService)
        public PersonsGetterService(IPersonsRepository personRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
        {
            //ilogger
            _logger = logger;
            //ilogger

            //idiagnosticcontext
            _diagnosticContext = diagnosticContext;
            //idiagnosticcontext

            _personRepository = personRepository;
            //_countriesService = countriesService;

            //_persons = new List<Person>();
            //_countriesService = new CountriesService();
            //if (initialize)
            //{
            //    //{}
            //    //{}
            //    //{}
            //    //{E1F0C660-B97A-497F-9F7B-3EA313063C3C}

            //    //sampel data
            //    // John Doe, john.doe @email.com,1985 - 07 - 15,Male,123 Main St, TRUE
            //    //Jane Smith, jane.smith @email.com,1990 - 03 - 22,Female,456 Oak Ave, FALSE
            //    //Bob Johnson, bob.johnson @email.com,1978 - 11 - 05,Male,789 Pine Rd, TRUE
            //    //Alice Brown, alice.brown @email.com,1982 - 09 - 18,Female,101 Cedar Ln, FALSE
            //    //Chris Wilson, chris.wilson @email.com,1995 - 04 - 30,Non - Binary,202 Elm Blvd, TRUE
            //    //sample data

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("C8CB6F80-9645-41D8-A6A5-A991EA1ACF06"),

            //        PersonName = "John Doe",
            //        Email = "john.doe @email.com",
            //        DateOfbirth = DateTime.Parse("1985-07-15"),
            //        Address = "123 Main St",
            //        ReceiveNewsLetters = true,
            //        Gender = "Male",
            //        CountryId =
            //        Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D")
            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

            //        PersonName = "Jane Smith",
            //        Email = "jane.smith @email.com",
            //        DateOfbirth = DateTime.Parse("1990-03-22"),
            //        Address = "456 Oak Ave",
            //        ReceiveNewsLetters = true,
            //        Gender = "Female",
            //        CountryId =
            //        Guid.Parse("52BC74D6-9094-441F-A51D-C1604F26F376")

            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("2EFA7A85-7252-4408-A377-CDE1BD1510F3"),

            //        PersonName = "Jane Smith",
            //        Email = "jane.smith @email.com",
            //        DateOfbirth = DateTime.Parse("1990-03-22"),
            //        Address = "456 Oak Ave",
            //        ReceiveNewsLetters = true,
            //        Gender = "Female",
            //        CountryId =
            //        Guid.Parse("9884B371-7E2F-4BC1-BD65-E34301D0AD2D")
            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

            //        PersonName = "Bob Johnson",
            //        Email = "bob.johnson @email.com",
            //        DateOfbirth = DateTime.Parse("1987-03-09"),
            //        Address = "789 Pine Rd",
            //        ReceiveNewsLetters = false,
            //        Gender = "male",
            //        CountryId =
            //        Guid.Parse("E45C96A1-3D83-4EF9-99E4-2B6FF51B5399")
            //    });

            //    _persons.Add(new Person()
            //    {
            //        Personid = Guid.Parse("3B86EE5F-5128-4BA4-BA37-63AD8B1C8BF7"),

            //        PersonName = "Alice Brown",
            //        Email = "alice.brown @email.com",
            //        DateOfbirth = DateTime.Parse("1982-09-18"),
            //        Address = "101 Cedar Ln",
            //        ReceiveNewsLetters = true,
            //        Gender = "Female",
            //        CountryId =
            //       Guid.Parse("54DEAD56-5A92-4C41-AF81-F27DC37BC67D")
            //    });
            //}
        }




        //public async Task<PersonResponse> AddPerson(PersonAddRequest personAddRequest)
        //{
        //    //CHECK IF PEROSNREQUEST OBJEVCT IS NOT NULL
        //    if (personAddRequest == null)
        //    {
        //        throw new ArgumentNullException(nameof(personAddRequest));
        //    }
        //    //if (string.IsNullOrEmpty(personAddRequest.PersonName))
        //    //{
        //    //    throw new ArgumentException("person name cannot be blank !");
        //    //}

        //    //mode validations:
        //    ValidationHelper.ModelValidation(personAddRequest);

        //    Person person = personAddRequest.ToPerson();
        //    person.Personid = Guid.NewGuid();
        //    //_persons.Add(person);

        //    //_personRepository.Add(person);
        //    //_personRepository.SaveChanges();

        //    await _personRepository.AddPerson(person);

        //    //_db.sp_InsertPersons(person);


        //    //convert perosn object into perosnrepsonse.
        //    return person.ToPersonResponse();
        //}

        public virtual async Task<List<PersonResponse>> GetAllPersons()
        {
            //ilogger
            _logger.LogInformation("GetAllperson of personservice");
            //ilogger

            //return _persons.Select(temp => temp.ToPersonResponse()).ToList();

            //var persons = _personRepository.Persons.Include("Country").ToList(); // if we dont do this then this property value will contain null. if you want to load navigation property or parent record details then use this include.

            var persons = await _personRepository.GetAllPersons();

            //var persons = _db.Persons.ToList();

            return persons.Select(temp => temp.ToPersonResponse()).ToList();

            //return _db.sp_GetAllPersons().Select(temp => ConvertPersonToPersonresponse(temp)).ToList();
        }

        public virtual async Task<PersonResponse>? GetpersonByPersonId(Guid? personid)
        {
            if (personid == null)
                return null;
            //Person? person = _persons.FirstOrDefault(temp => temp.Personid == personid);
            //Person? person = _db.Persons.FirstOrDefault(temp => temp.Personid == personid);
            //Person? person = _personRepository.Persons.Include("Country").FirstOrDefault(temp => temp.Personid == personid);

            Person? person = await _personRepository.GetPersonByPersonID(personid.Value);
            if (person == null)
                return null;

            return person.ToPersonResponse();

        }

        public virtual async Task<List<PersonResponse>> GetFilteredpersons(string searchby, string? searchString)
        {
            //ilogger
            _logger.LogInformation("GetFilteredpersons of personservice");
            //ilogger

            //List<PersonResponse> allPersons = await GetAllPersons();


            //serilog timings
            List<Person> persons = null;
            using (Operation.Time("Time for executing GetFilteredpersons "))
            {
                //serilog timings

                persons = searchby switch
                //List<PersonResponse> matchingPersons = allPersons;

                //if (string.IsNullOrEmpty(searchby) || string.IsNullOrEmpty(searchString))
                //    return matchingPersons;

                //switch (searchby)
                //{
                //    case nameof(PersonResponse.PersonName):
                //        matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                //        break;

                //    case nameof(PersonResponse.Email):
                //        matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                //        break;

                //    case nameof(Person.DateOfbirth):
                //        matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                //        break;

                //    case nameof(PersonResponse.Gender):
                //        matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Equals(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                //        break;

                //    case nameof(PersonResponse.CountryId):
                //        matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                //        break;


                //    case nameof(PersonResponse.Address):
                //        matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                //        break;

                //    case nameof(PersonResponse.Country):
                //        matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                //        break;

                //    default: matchingPersons = allPersons; break;

                //}

                {
                    nameof(PersonResponse.PersonName) =>
                       await _personRepository.GetFilteredPersons(temp => temp.PersonName.Contains(searchString)),


                    nameof(PersonResponse.Email) =>
                       await _personRepository.GetFilteredPersons(temp => temp.Email.Contains(searchString)),


                    nameof(PersonResponse.DateOfBirth) =>
                       await _personRepository.GetFilteredPersons(temp => temp.DateOfbirth.Value.ToString("dd MMM yyyy").Contains(searchString)),


                    nameof(PersonResponse.Gender) =>
                      await _personRepository.GetFilteredPersons(temp => temp.Gender.Contains(searchString)),

                    nameof(PersonResponse.CountryId) =>
                       await _personRepository.GetFilteredPersons(temp => temp.Country.CountryName.Contains(searchString)),


                    nameof(PersonResponse.Address) =>
                       await _personRepository.GetFilteredPersons(temp => temp.Address.Contains(searchString)),



                    _ => await _personRepository.GetAllPersons()

                };
            } // serilog timings using block 

            //idiagnosticcontext : you will see log with endpoint completion and inthat we will see this value
            _diagnosticContext.Set("Persons", persons);
            //idiagnosticcontext

            return persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        //public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        //{
        //    //ilogger
        //    _logger.LogInformation("GetSortedPersons of personservice");
        //    //ilogger

        //    if (string.IsNullOrEmpty(sortBy))
        //        return allPersons;

        //    List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
        //    {
        //        (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

        //        (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

        //        (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

        //        (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

        //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

        //        (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

        //        (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

        //        (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

        //        (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender).ToList(),

        //        (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender).ToList(),

        //        (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country).ToList(),

        //        (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country).ToList(),

        //        (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address).ToList(),

        //        (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address).ToList(),

        //        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

        //        (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

        //        _ => allPersons //_ means deafult case.

        //    };  // switch expression
        //    return sortedPersons;

        //}

        //public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        //{
        //    if (personUpdateRequest == null)
        //    {
        //        //throw new ArgumentNullException(nameof(Person));
        //        throw new InvalidPersonIDException("Given person id doesnt exists !...");
        //    }
        //    ValidationHelper.ModelValidation(personUpdateRequest);

        //    //get matching person object to update
        //    //Person? matchingPerson = _persons.FirstOrDefault(temp => temp.Personid == personUpdateRequest.PersonId);

        //    //Person? matchingPerson = await _personRepository.Persons.FirstOrDefaultAsync(temp => temp.Personid == personUpdateRequest.PersonId);

        //    Person? matchingPerson = await _personRepository.GetPersonByPersonID(personUpdateRequest.PersonId);

        //    if (matchingPerson == null)
        //        throw new ArgumentException("Given person id doesnt exist !");

        //    //update all details
        //    matchingPerson.PersonName = personUpdateRequest.PersonName;
        //    matchingPerson.Email = personUpdateRequest.Email;
        //    matchingPerson.DateOfbirth = personUpdateRequest.DateOfBirth;
        //    matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        //    matchingPerson.CountryId = personUpdateRequest.CountryId;
        //    //await _personRepository.SaveChangesAsync(); //update 

        //    await _personRepository.UpdatePerson(matchingPerson);
        //    return matchingPerson.ToPersonResponse();
        //}

        //public async Task<bool> DeletePerson(Guid? personId)
        //{
        //    if (personId == null)
        //        throw new ArgumentNullException(nameof(personId));
        //    //Person person = _persons.FirstOrDefault(temp => temp.Personid == personId);
        //    //Person? person = _personRepository.Persons.FirstOrDefault(temp => temp.Personid == personId);

        //    Person? person = await _personRepository.GetPersonByPersonID(personId.Value);

        //    if (person == null)
        //        return false;

        //    //_persons.RemoveAll(temp => temp.Personid == personId);
        //    //_personRepository.Persons.Remove(_personRepository.Persons.First(temp => temp.Personid == personId));

        //    await _personRepository.DeletePersonByPersonID(personId.Value);
        //    //await _personRepository.SaveChangesAsync();
        //    return true;
        //}

        public virtual async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream); // satreamwriter writes content into memorystream

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));



            //CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true); // leaveopen: true is for , after writing we have to start from beginning for writing same into the file.
            //csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord(); // adds \n new line into the file
            //List<PersonResponse> persons = _personRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();

            List<PersonResponse> persons = await GetAllPersons();

            foreach (PersonResponse person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth.HasValue)
                {
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                }
                else
                    csvWriter.WriteField("");
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.ReceiveNewsLetters);
                csvWriter.NextRecord();
                csvWriter.Flush(); // for data return by csvwriter will be added to memorystream.
            }

            //await csvWriter.WriteRecordsAsync(persons);

            memoryStream.Position = 0;
            return memoryStream;
        }

        public virtual async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream(); //it can contain image/excel/csv i.e. any type of data of file.

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date Of Birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";
                worksheet.Cells["H1"].Value = "Receive News Letters";

                using (ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                //List<PersonResponse> persons = _personRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();

                List<PersonResponse> persons = await GetAllPersons();

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
                    worksheet.Cells[row, 4].Value = personResponse.Age;
                    worksheet.Cells[row, 5].Value = personResponse.Gender;
                    worksheet.Cells[row, 6].Value = personResponse.Country;
                    worksheet.Cells[row, 7].Value = personResponse.Address;
                    worksheet.Cells[row, 8].Value = personResponse.ReceiveNewsLetters;

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