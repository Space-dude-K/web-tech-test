namespace Entities.RequestFeatures.User
{
    public class UserParameters : RequestParameters
    {
        public int MinId { get; set; }
        public int MaxId { get; set; } = int.MaxValue;
        public bool ValidIdRange => MaxId > MinId;

        public int MinNameLength { get; set; }
        public int MaxNameLength { get; set; } = int.MaxValue;
        public bool ValidNameRange => MaxNameLength > MinNameLength;

        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;
        public bool ValidAgeRange => MaxAge > MinAge;

        public int MinEmailLength { get; set; }
        public int MaxEmailLength { get; set; } = int.MaxValue;
        public bool ValidEmailRange => MaxEmailLength > MinEmailLength;

        public UserParameters(string fields)
        {
            Fields = fields;
        }
        public UserParameters()
        {
            OrderBy = "Name";
        }
    }
}