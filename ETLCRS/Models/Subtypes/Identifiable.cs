using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ETLCRS.Constants;

namespace ETLCRS.Models.Subtypes
{
    /// <summary>
    /// Identity and position row type
    /// </summary>
    public class Identifiable : SubType
    {

        public Component<int> Identity { get; set; }

        public Component<string> ProjValue { get; set; }

        public Identifiable()
        {
            Identity = new Component<int>()
            {
                Start = Character.LEFT_ANGLE_BRACKET,
                End = Character.PROJ
            };
            ProjValue = new Component<string>()
            {
                Start = Character.PROJ,
                End = Character.ANGLE_BRACKETS
            };
        }

        ///<inheritdoc/>
        internal override List<int> NumberOfLine => new List<int> {3,4};
        ///<inheritdoc/>
        public override string Start  =>  Character.LEFT_ANGLE_BRACKET;
        ///<inheritdoc/>
        public override string Contains  =>  Character.PROJ;
        ///<inheritdoc/>
        public override string End  =>  Character.ANGLE_BRACKETS;
        ///<inheritdoc/>
        public override List<string> SplitsPatterns => new List<string> {"^<\\d+>"};

        ///<inheritdoc/>
        public override void ClearValues()
        {
            Identity.Value = default(int);
            ProjValue.Value = default(string);
        }

        ///<inheritdoc/>
        public override string FeedValues(string line)
        {
            var values = Regex.Split(line, $"({SplitsPatterns.FirstOrDefault()})");

            if (!ValidateSplit(values)) return line;

            var strId = values[1].Replace(Character.LEFT_ANGLE_BRACKET, string.Empty)
                                .Replace(Character.RIGHT_ANGLE_BRACKET,string.Empty)
                                .Trim();

            if (!int.TryParse(strId, out var identityValue)) return line;

            Identity.Value = identityValue;

            ProjValue.Value = values[2].Replace(Character.ANGLE_BRACKETS,string.Empty)
                                       .Trim();

            return string.IsNullOrWhiteSpace(ProjValue.Value)
                    ? line
                    : null;
        }

    }
}