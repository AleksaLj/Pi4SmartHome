
namespace AdminManagementDSL.AdminDSL.Common.Dto
{
    public class SqlTableDto
    {
        public string TableName { get; set; } = null!;
        public string TableType { get; set; } = null!;
        public List<SqlRowDto> Rows { get; set; } = new List<SqlRowDto>();
    }
}
