using FluentValidation;
using MovieStore.Models.Requests.Actor;

namespace MovieStore.Validators
{
    public class UpdateActorRequestValidator : AbstractValidator<UpdateActorRequest>
    {
        public UpdateActorRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("Actor ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .MinimumLength(2)
                .WithMessage("Actor name must be between 2 and 50 characters.");
        }
    }
}
