using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace CrudTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory> // for creating object of CustomWebApplicationFactory automatically
    {
        private readonly HttpClient _client;

        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region index
        [Fact]
        public async void Index_ToReturnView()
        {
            //arrange


            //act
            HttpResponseMessage response = await _client.GetAsync("/Persons/Index");

            //assert
            response.Should().BeSuccessful();

            //for getting responsebody
            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            var document = html.DocumentNode;
            document.QuerySelectorAll("table.persons").Should().NotBeNull();

        }
        #endregion
    }
}
