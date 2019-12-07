using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ETLCRS.Models
{
    public abstract class ComponentSymbols
    {
        public virtual string Start { get; set; }

        public virtual string Contains { get; set; }

        public virtual string End { get; set; }

        public virtual List<string> SplitsPatterns { get; set; }
    }
}