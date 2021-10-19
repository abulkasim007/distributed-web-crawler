namespace DataStructures
{
    public class Url
    {
        public Url(int depth, string absoluteUri)
        {
            this.Depth = depth;
            this.AbsoluteUri = absoluteUri;
        }

        public int Depth { get; set; }

        public string AbsoluteUri { get; set; }
    }
}
