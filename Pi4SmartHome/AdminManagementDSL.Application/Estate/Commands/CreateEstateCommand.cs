using MediatR;

namespace AdminManagementDSL.Application.Estate.Commands
{
    public record CreateEstateCommand(string Name,
                                      string? Addr,
                                      string? Description,
                                      byte EstateTypeId) : IRequest<int>;
}
