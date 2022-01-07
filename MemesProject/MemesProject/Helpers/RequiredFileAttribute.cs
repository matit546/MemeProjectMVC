using System.ComponentModel.DataAnnotations;

namespace MemesProject.Helpers
{
    public class RequiredFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            string? stringValue = value as string;
            if (stringValue != null && !AllowEmptyStrings)
            {
                return stringValue.Trim().Length != 0;
            }
            else
            {
                return true;
            }
        }
    }
}
