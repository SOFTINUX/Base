using System.Collections.Generic;

namespace Infrastructure
{
    public class MenuItem
    {
        public string Url {get; set;}
        public string Name {get; set;}
        public int Position {get;}

        //TODO here place properties for menu access permissions
        //public IEnumerable<string> PermissionCodes { get; set; }

        //public MenuItem(string url_, string name_, int position_, IEnumerable<string> permissionCodes_)
        public MenuItem(string url_, string name_, int position_)
        {
            Url = url_;
            Name = name_;
            Position = position_;
            // PermissionCodes = permissionCodes_;
        }
    }

}