using FluentValidation;

namespace SupportCenter.Application.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandValidator
    : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}