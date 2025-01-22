using FluentValidation;
using MovieStore.Models.Requests.Actor;

namespace MovieStore.Validators
{
    public class AddActorRequestValidator : AbstractValidator<AddActorRequest>
    {
        public AddActorRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithMessage("Actor name must be between 2 and 50 characters.");
        }
    }
}
