using BookStore.Models.Requests.Book;
using FluentValidation;

namespace BookStore.Validators.Book
{
    public class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookRequestValidator()
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

            RuleForEach(x => x.Authors)
                .NotEmpty()
                .NotNull()
                .WithMessage("Author IDs cannot be null or empty.");
        }

    }
}
