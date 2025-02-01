using BookStore.Models.Requests.Author;
using FluentValidation;

namespace BookStore.Validators
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("Author ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithMessage("Author name must be between 2 and 50 characters.");
        }
    }
}
