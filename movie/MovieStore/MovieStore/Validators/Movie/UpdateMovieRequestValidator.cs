using FluentValidation;
using MovieStore.Models.Requests.Movie;

namespace MovieStore.Validators.Movie
{
    public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
    {
        public UpdateMovieRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("Movie ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .MinimumLength(2)
                .WithMessage("Title must be between 2 and 100 characters."); ;

            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("Year must be greater than 1900 lshfkjsd")
                .LessThan(2100);
        }
    }
}
