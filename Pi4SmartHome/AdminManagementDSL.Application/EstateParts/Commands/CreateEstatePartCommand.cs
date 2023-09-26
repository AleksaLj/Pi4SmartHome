using MediatR;

namespace AdminManagementDSL.Application.EstateParts.Commands
{
    public record CreateEstatePartCommand(string? EstatePartName, int EstateId) : IRequest<int>;
}
