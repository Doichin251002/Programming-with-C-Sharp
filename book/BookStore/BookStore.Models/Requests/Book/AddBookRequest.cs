namespace BookStore.Models.Requests.Book
{
    public class AddBookRequest
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public List<string> Authors { get; set; }
    }
}
