
namespace AdminManagementDSL.Core.Entities
{
    public class Estates
    {
        public int EstateId { get; set; }

        public string Name { get; set; } = null!;

        public string? Addr { get; set; }

        public string? Description { get; set; }

        public byte EstateTypeId { get; set; }

        public List<Users>? Users { get; set; }
    }
}
