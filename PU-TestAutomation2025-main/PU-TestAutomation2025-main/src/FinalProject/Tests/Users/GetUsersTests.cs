using FluentAssertions;
using Newtonsoft.Json;

namespace dotNetFinalProjectAPITesting.Tests.Users;

[TestFixture]
public class GetUsersTests : TestBase
{
    [Test]
    [Parallelizable]
    public async Task Get_All_Users_Returns_OK()
    {
        var response = await Client.GetAsync("users");

        response.StatusCode.ToString().Should().Be("OK");

        string responseData = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<UserResponse>>(responseData);

        users.Should().NotBeNull();
        users.Count().Should().Be(10);
        foreach (var user in users)
        {
            user.id.Should().BePositive();
            user.name.Should().NotBeNullOrWhiteSpace(); 
            user.email.Should().NotBeNullOrWhiteSpace();
            user.status.Should().NotBeNullOrWhiteSpace();
            user.gender.Should().NotBeNullOrWhiteSpace();
        }
    }
    
    [Test]
    [Parallelizable]
    public async Task Get_User_By_Id_Returns_OK()
    {
        var response = await Client.GetAsync("users/7890027");

        response.StatusCode.ToString().Should().Be("OK");

        string responseData = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<UserResponse>(responseData);

        user.Should().NotBeNull();
        user.id.Should().Be(7890027);
        user.name.Should().Be("Fiki Storaro");
        user.email.Should().Be("testovi1@test.com");
        user.gender.Should().Be("male");
        user.status.Should().Be("active");
    }
    
    [Test]
    [Parallelizable]
    public async Task Get_User_By_Nonexistent_Id_Returns_NotFound()
    {
        var response = await Client.GetAsync("users/0");

        response.StatusCode.ToString().Should().Be("NotFound");

        string responseData = await response.Content.ReadAsStringAsync();

        responseData.Should().Contain("Resource not found");
    }
}