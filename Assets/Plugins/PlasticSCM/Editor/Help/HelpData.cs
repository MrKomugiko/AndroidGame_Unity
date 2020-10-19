using System.Collections.Generic;

namespace PlasticGui.Help
{
    public class HelpData
    {
        public List<HelpFormat> FormattedBlocks = new List<HelpFormat>();
        public List<HelpLink> Links = new List<HelpLink>();
        public string CleanText;
        public bool ShouldShowAgain;
    }
}