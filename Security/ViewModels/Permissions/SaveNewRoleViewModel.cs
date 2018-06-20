using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when adding a new role.
    /// </summary>
    public class SaveNewRoleViewModel : IValidatableObject
    {
        public string Role {get; set;}

        public List<string> Extensions {get; set;}

        public string Permission {get; set;}

        /// <summary>
        /// Return the validation errors when the validation gets called (call to Model.IsValid()).
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           // TODO SOFB-31 call a method to check that the role name doesn't already exist (this will be used at several places so use a toolbox)
            // yield return new ValidationResult(errorMessage, "Role")
           return null;
        }
    }
}