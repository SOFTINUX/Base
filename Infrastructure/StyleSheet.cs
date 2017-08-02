namespace Infrastructure
{
    public class StyleSheet
    {
        public string Url {get; set;}
        public int Position {get;}

        public StyleSheet(string url_, int position_)
        {
            Url = url_;
            Position = position_;
        }
    }
}