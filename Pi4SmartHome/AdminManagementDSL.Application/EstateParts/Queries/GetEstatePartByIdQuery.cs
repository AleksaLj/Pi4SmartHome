using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.EstateParts.Queries
{
    public record GetEstatePartByIdQuery(int id) : IRequest<EstatePart?>;
}
