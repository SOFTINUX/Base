namespace Infrastructure
{
    public class Script
    {
        public string Url {get; set;}
        public int Position {get;}

        public Script(string url_, int position_)
        {
            Url = url_;
            Position = position_;
        }
    }
}