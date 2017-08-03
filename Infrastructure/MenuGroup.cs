using System.Collections.Generic;

namespace Infrastructure
{
    public class MenuGroup
    {
        public string Name {get; set;}
        public int Position {get;}

        public IEnumerable<MenuItem> MenuItems {get;}

        public MenuGroup(string name_, int position_, IEnumerable<MenuItem> menuItems_)
        {
            Name = name_;
            Position = position_;
            MenuItems = menuItems_;
        }
    }
}