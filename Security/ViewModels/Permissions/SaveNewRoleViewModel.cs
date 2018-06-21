using System.Collections.Generic;


namespace Security.ViewModels.Permissions
{
    /// <summary>
    /// The posted data when adding a new role.
    /// </summary>
    public class SaveNewRoleViewModel
    {
        public string Role {get; set;}

        public List<string> Extensions {get; set;}

        public string Permission {get; set;}
    }
}