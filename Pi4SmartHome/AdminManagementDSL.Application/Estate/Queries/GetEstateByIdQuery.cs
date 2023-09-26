using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.Estate.Queries
{
    public record GetEstateByIdQuery(int Id) : IRequest<Estates?>;
}
