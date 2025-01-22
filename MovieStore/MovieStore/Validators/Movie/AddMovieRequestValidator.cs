using FluentValidation;
using MovieStore.Models.Requests.Movie;

namespace MovieStore.Validators.Movie
{
    public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
    {
        public AddMovieRequestValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .MinimumLength(2)
                .WithMessage("Title must be between 2 and 100 characters.");

            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("Year must be greater than 1900.")
                .LessThan(2100).WithMessage("Year must be less than 2100.");

            RuleForEach(x => x.Actors)
                .NotEmpty()
                .NotNull()
                .WithMessage("Actor IDs cannot be null or empty.");
        }

    }
}
