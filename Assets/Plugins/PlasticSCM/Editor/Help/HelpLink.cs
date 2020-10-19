namespace PlasticGui.Help
{
    public class HelpLink
    {
        public enum LinkType
        {
            Action,
            Help,
            Link,
        }

        public int Position;
        public int Length;
        public string Link;

        public LinkType Type;
    }
}