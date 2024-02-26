using AutoFixture;
using Moq;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrudExample.Controllers;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services;
using Microsoft.Extensions.Logging;

namespace CrudTests
{
    public class PersonControllerTest
    {
        //private readonly IPersonsService _personsService;
        private readonly IPersonsAdderService _personsAdderService;
        private readonly IPersonsGetterService _personsGetterService;
        private readonly IPersonsDeleterService _personsDeleterService;
        private readonly IPersonsUpdaterService _personsUpdaterService;
        private readonly IPersonsSorterService _personsSorterService;


        private readonly ICountriesService _countriesService;
        //private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly Mock<IPersonsAdderService> _personsAdderServiceMock;
        private readonly Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;
        private readonly Mock<IPersonsDeleterService> _personsDeleterServiceMock;
        private readonly Mock<IPersonsSorterService> _personsSorterServiceMock;
        private readonly Mock<IPersonsGetterService> _personsGetterServiceMock;

        private readonly Mock<ICountriesService> _countriesServiceMock;

        private readonly Fixture _fixture;
        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;


        public PersonControllerTest()
        {
            _fixture = new Fixture();
            _countriesServiceMock = new Mock<ICountriesService>();
            //_personsServiceMock = new Mock<IPersonsService>();
            _personsAdderServiceMock = new Mock<IPersonsAdderService>();
            _personsGetterServiceMock = new Mock<IPersonsGetterService>();
            _personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();
            _personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
            _personsSorterServiceMock = new Mock<IPersonsSorterService>();

            _loggerMock = new Mock<ILogger<PersonsController>>();

            _countriesService = _countriesServiceMock.Object;
            //_personsService = _personsServiceMock.Object;

            _personsAdderService = _personsAdderServiceMock.Object;
            _personsGetterService = _personsGetterServiceMock.Object;
            _personsUpdaterService = _personsUpdaterServiceMock.Object;
            _personsDeleterService = _personsDeleterServiceMock.Object;
            _personsSorterService = _personsSorterServiceMock.Object;
            _logger = _loggerMock.Object;


        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithValidPerson()
        {
            //arrange
            List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();
            PersonsController personsController = new PersonsController(_personsGetterService, _personsAdderService,_personsUpdaterService, _personsSorterService, _personsDeleterService, _countriesService, _logger);

            _personsGetterServiceMock.Setup(temp => temp.GetFilteredpersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);

            _personsSorterServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(persons_response_list);

            _personsGetterServiceMock.Setup(temp => temp.GetFilteredpersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons_response_list);

            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            //assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();

            viewResult.ViewData.Model.Should().Be(persons_response_list);
        }

        #endregion

        #region
        [Fact]
        public async void CreateIfModelErrors_ToReturnCreateView()
        {
            //arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);


            //PersonsController personsController = new PersonsController(_personsService, _countriesService);

            PersonsController personsController = new PersonsController(_personsGetterService, _personsAdderService, _personsUpdaterService, _personsSorterService, _personsDeleterService, _countriesService, _logger);

            _personsAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            //act
            personsController.ModelState.AddModelError("PersonName", "Person Name cannot be blank !");

            IActionResult result = await personsController.Create(person_add_request);

            //assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            //viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonAddRequest>>();

            viewResult.ViewData.Model.Should().Be(person_add_request);
        }

        [Fact]
        public async void CreateIfNoModelErrors_ToReturnRedirectToIndex()
        {
            //arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);


            //PersonsController personsController = new PersonsController(_personsService, _countriesService);

            PersonsController personsController = new PersonsController(_personsGetterService, _personsAdderService, _personsUpdaterService, _personsSorterService, _personsDeleterService, _countriesService, _logger);

            _personsAdderServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(person_response);

            //act
            //personsController.ModelState.AddModelError("PersonName", "Person Name cannot be blank !");

            IActionResult result = await personsController.Create(person_add_request);

            //assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be("Index") ;

           
        }
        #endregion

    }
}
