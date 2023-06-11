using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace RemindMeal.Tests
{
    public sealed class BasicTests
    {
        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/Privacy")]
        [InlineData("/Recipes")]
        [InlineData("/Recipes/Create")]
        [InlineData("/Meals")]
        [InlineData("/Meals/Create")]
        [InlineData("/Friends")]
        [InlineData("/Friends/Create")]
        [InlineData("/Identity/Account/Manage")]
        [InlineData("/Identity/Account/Manage/ChangePassword")]
        [InlineData("/Identity/Account/Manage/TwoFactorAuthentication")]
        [InlineData("/Identity/Account/Manage/PersonalData")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var factory = new WebApplicationFactory<Program>();
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }
    }
    
}
