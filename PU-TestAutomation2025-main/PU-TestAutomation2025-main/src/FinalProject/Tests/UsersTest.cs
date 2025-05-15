using dotNetFinalProjectAPITesting;
using FluentAssertions;
using Newtonsoft.Json;

namespace FinalProject.Tests
{
    [TestFixture]
    public class UsersTests : TestBase
    {
        [Test]
        public async Task GetUsers()
        {
            var response = await Client.GetAsync("users");

            //Example fluentassertion
            response.StatusCode.ToString().Should().Be("OK");

            string responseData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserResponse>>(responseData);

            users.Should().NotBeNull();
            users.Count().Should().Be(10);
        }
    }
}
