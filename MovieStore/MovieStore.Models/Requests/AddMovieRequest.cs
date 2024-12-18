namespace MovieStore.Models.Requests
{
    public class AddMovieRequest
    {
        public string Title { get; set; }

        public int Year { get; set; }

        public List<string> Actors { get; set; }
    }
}
