using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Validation
{
    public class EntityValidationResult
    {
        public IList<ValidationResult> Errors { get; private set; }
		
        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }

        public EntityValidationResult(IList<ValidationResult> errors = null)
        {
            Errors = errors ?? new List<ValidationResult>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Errors);
        }
    }
}
