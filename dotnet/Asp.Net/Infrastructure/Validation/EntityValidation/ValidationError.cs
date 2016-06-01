using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Infrastructure.Validation
{
    public class ValidationErrors
    {
        public ValidationErrors()
        {
            Errors = new List<ValidationError>();
        }

        public List<ValidationError> Errors { get; set; }

        public override string ToString()
        {
            return String.Join(",<br>", Errors.Select(e => e.Error));
        }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string NewValue { get; set; }
        public string Error { get; set; }
        public bool IsAntiXSS { get; set; }
    }
}