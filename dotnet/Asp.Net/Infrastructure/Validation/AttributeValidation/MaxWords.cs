using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infrastructure.Validation.Attributes
{
    public class MaxWords : ValidationAttribute
    {
        public int Max { get; set; }

        public override bool IsValid(object value)
        {
            var val = value as string;
            if (!String.IsNullOrWhiteSpace(val))
            {
                return val.Trim().Split(' ').Count() <= Max;
            }

            return true;
        }
    }
}