namespace Entities.RequestFeatures.User
{
    public class UserParameters : RequestParameters
    {
        public int MinUserId { get; set; }
        public int MaxUserId { get; set; } = int.MaxValue;
        public bool ValidIdRange => MaxUserId > MinUserId;

        public int MinUserNameLength { get; set; }
        public int MaxUserNameLength { get; set; } = int.MaxValue;
        public bool ValidNameRange => MaxUserNameLength > MinUserNameLength;

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