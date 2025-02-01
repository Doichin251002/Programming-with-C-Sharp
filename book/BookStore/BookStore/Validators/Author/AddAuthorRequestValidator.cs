using BookStore.Models.Requests.Author;
using FluentValidation;

namespace BookStore.Validators
{
    public class AddAuthorRequestValidator : AbstractValidator<AddAuthorRequest>
    {
        public AddAuthorRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithMessage("Author name must be between 2 and 50 characters.");
        }
    }
}
