using FluentValidation;

namespace SupportCenter.Application.Features.Tickets.UpdateTicket;

public sealed class UpdateTicketValidator : AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title!)
                .NotEmpty()
                .MaximumLength(200);
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description!)
                .NotEmpty();
        });
    }
}
