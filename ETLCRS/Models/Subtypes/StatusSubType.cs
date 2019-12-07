using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ETLCRS.Constants;

namespace ETLCRS.Models.Subtypes
{
    /// <summary>
    /// Status Type of Position
    /// </summary>
    public class StatusSubType: SubType
    {
        public static List<string> StatusTypes = new List<string>{"DISCONTINUED", "DEPRECATED"};
        ///<inheritdoc/>
        public override string Start  =>  Character.HASH;
        ///<inheritdoc/>
        internal override List<int> NumberOfLine => new List<int> {3};
        ///<inheritdoc/>
        public override List<string> SplitsPatterns => new List<string> { "^#[\\s][^\\s]+" };

        public Component<string> Status { get; set; } = new Component<string>()
                                                            {
                                                                SplitsPatterns = new List<string>()
                                                                {
                                                                    StatusTypes.Aggregate((x,y) => $"{x}|{y}")
                                                                }
                                                            };

        ///<inheritdoc/>
        public override void ClearValues()
        {
            Status.Value = default(string);
        }

        ///<inheritdoc/>
        public override bool GetSubType(KeyValuePair<int,SubType> subtype, string line) => NumberOfLine.Contains(subtype.Key) 
                                                                                            && line.StartsWith(this.Start) 
                                                                                            && StatusTypes.Any(line.Contains);

        ///<inheritdoc/>
        public override string FeedValues(string line)
        {
            var values = Regex.Split(line, $"({SplitsPatterns.FirstOrDefault()})");

            if (!ValidateSplit(values)) return line;

            Status.Value = values[1].Replace(Character.HASH, string.Empty)
                                    .Replace(Character.COLON, string.Empty)
                                    .Trim();

            return string.IsNullOrWhiteSpace(Status.Value) ? line : null;
        }

        ///<inheritdoc/>
        public override bool ValidateSplit(string[] values)
        {
            return values.Length == 3 
                   && values.Where(string.IsNullOrWhiteSpace).Count() <= 2;
        }
    }
}