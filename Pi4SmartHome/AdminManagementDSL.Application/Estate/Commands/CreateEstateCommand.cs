using MediatR;

namespace AdminManagementDSL.Application.Estate.Commands
{
    public record CreateEstateCommand(string name,
                                      string? addr,
                                      string? description,
                                      byte estateTypeId) : IRequest<int>;
}
