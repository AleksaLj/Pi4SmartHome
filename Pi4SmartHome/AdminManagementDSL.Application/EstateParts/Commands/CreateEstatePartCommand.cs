using MediatR;

namespace AdminManagementDSL.Application.EstateParts.Commands
{
    public record CreateEstatePartCommand(string? estatePartName, int estateId) : IRequest<int>;
}
