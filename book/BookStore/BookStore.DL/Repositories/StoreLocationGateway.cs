using BookStore.DL.Interfaces;
using RestSharp;

namespace BookStore.DL.Repositories
{
    public class StoreLocationGateway : IStoreLocationGateway
    {
        private readonly RestClient _client;

        public StoreLocationGateway()
        {
            var options = new RestClientOptions("https://localhost:7246");

            _client = new RestClient(options);
        }

        public async Task<string> GetAllLocations()
        {
            var request = new RestRequest($"/location", Method.Get);

            var response = await _client.ExecuteAsync(request);

            return response.Content.ToString();
        }
    }
}
