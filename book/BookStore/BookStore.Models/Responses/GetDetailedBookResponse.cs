using BookStore.Models.POCO;

namespace BookStore.Models.Responses
{
    public class GetDetailedBookResponse
    {
        public Book Book { get; set; }

        public List<Author> Authors { get; set; }
    }
}
