using System.Collections.Generic;

namespace Infrastructure
{
    public class GroupMenuItem
    {
        public string Url {get; set;}
        public string Name {get; set;}
        public int Position {get;}

        public IEnumerable<MenuItem> MenuItems {get;}

        public GroupMenuItem(string url_, string name_, int position_, IEnumerable<MenuItem> menuItems_)
        {
            Url = url_;
            Name = name_;
            Position = position_;
            MenuItems = menuItems_;
        }
    }
}