namespace Entities.RequestFeatures.Role
{
    public class RoleParameters  : RequestParameters
    {
        public int MinRoleId { get; set; }
        public int MaxRoleId { get; set; } = int.MaxValue;
        public bool ValidIdRange => MaxRoleId > MinRoleId;

        public int MinRoleNameLength { get; set; }
        public int MaxRoleNameLength { get; set; } = int.MaxValue;
        public bool ValidNameRange => MaxRoleNameLength > MinRoleNameLength;

        public RoleParameters(string fields)
        {
            Fields = fields;
        }
        public RoleParameters()
        {
            OrderBy = "Name";
        }
    }
}