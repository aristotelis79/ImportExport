using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ETLCRS.Constants;

namespace ETLCRS.Models.Subtypes
{
    /// <summary>
    /// Name and describe Component row
    /// </summary>
    public class Describable : SubType
    {
        internal override List<int> NumberOfLine => new List<int> { 1 };

        public override string Start => Character.HASH;
        public override string Contains => Character.LEFT_SQUARE_BRACKET;
        public override string End => Character.RIGHT_SQUARE_BRACKET;

        public override List<string> SplitsPatterns => new List<string> { "^#[\\s][^\\s]+" };

        public Describable()
        {
            Name = new Component<string>()
            {
                Start = Character.HASH,
                End = Character.LEFT_SQUARE_BRACKET
            };
            Description = new Component<string>()
            {
                Start = Character.LEFT_SQUARE_BRACKET,
                End = Character.RIGHT_SQUARE_BRACKET
            };
        }

        public override void ClearValues()
        {
            Name.Value = default(string);
            Description.Value = default(string);
        }

        ///<inheritdoc/>
        public override bool GetSubType(KeyValuePair<int, SubType> subtype, string line) => NumberOfLine.Contains(subtype.Key)
                                                                                                    && (string.IsNullOrWhiteSpace(this.Start) || line.StartsWith(this.Start))
                                                                                                    && (string.IsNullOrWhiteSpace(this.Contains) || line.Contains(this.Contains))
                                                                                                    && (string.IsNullOrWhiteSpace(this.End) || line.EndsWith(this.End))
                                                                                                    && Regex.IsMatch(line, SplitsPatterns.First());

        public Component<string> Name { get; set; }

        public Component<string> Description { get; set; }


        ///<inheritdoc/>
        public override string FeedValues(string line)
        {
            var values = Regex.Split(line, $"({SplitsPatterns.FirstOrDefault()})");

            if (!ValidateSplit(values)) return line;

            var nameTemp = values[1].Trim();
            var description = values[2].Trim();

            Name.Value = nameTemp.Substring(1,nameTemp.Length-1).Trim();
            Description.Value = description.Trim().Substring(1,description.Length-2);

            return string.IsNullOrWhiteSpace(Name.Value)
                   || string.IsNullOrWhiteSpace(Description.Value)
                ? line
                : null;
        }
    }
}