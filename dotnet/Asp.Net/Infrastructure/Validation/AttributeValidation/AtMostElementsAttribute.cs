using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Validation.Attributes
{
    public class AtMostElementsAttribute : ValidationAttribute
    {
        public int Max { get; set; }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count <= Max;
            }

            return true;
        }
    }
}
