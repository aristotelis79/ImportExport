using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ETLCRS.Constants;

namespace ETLCRS.Models.Subtypes
{
    /// <summary>
    /// Area type of line
    /// </summary>
    public class Area : SubType
    {
        internal override List<int> NumberOfLine => new List<int> {2};

        public override string Start  =>  Character.HASH;
        public override string Contains  =>  Character.AREA;
        public override string End  =>  Character.RIGHT_SQUARE_BRACKET;
        
        public override List<string> SplitsPatterns => new List<string>
        {
            "\\(lat:[\\s]-?\\d*\\.?\\d*,[\\s]-?\\d*\\.?\\d*\\)",
            "\\(lon:[\\s]-?\\d*\\.?\\d*,[\\s]-?\\d*\\.?\\d*\\)"
        };


        public Area()
        {
            Lat = new Component<Point<decimal>>
            {
                Start = Character.LAT,
                Contains = Character.COMMA,
                End = Character.RIGHT_BRACKET,
                Value = new Point<decimal>()
            };

            Lon = new Component<Point<decimal>>
            {
                Start = Character.LON,
                Contains = Character.COMMA,
                End = Character.RIGHT_BRACKET,
                Value = new Point<decimal>()
            };
        }

        public Component<Point<decimal>> Lat { get; set; }

        public Component<Point<decimal>> Lon { get; set; }


        public class Point<T> : ComponentSymbols where T : new()
        {
            public Point()
            {
                Min = new T();
                Max = new T();
            }

            public T Min { get; set; }

            public T Max { get; set; }
        }

        ///<inheritdoc/>
        public override string FeedValues(string line)
        {
            var values = Regex.Split(line, $"({SplitsPatterns.FirstOrDefault()})");

            if (!ValidateSplit(values)) return line;

            if (!GetPoints(Lat, values[1])) return line;
            
            var nextValues = Regex.Split( values[2], $"({SplitsPatterns.Skip(1).FirstOrDefault()})");

            if (values.Length != 3) return line;

            if (!GetPoints(Lon, nextValues[1])) return line;

            return null;
        }



        public override void ClearValues()
        {
            Lat.Value.Min = default(decimal);
            Lat.Value.Max = default(decimal);
            Lon.Value.Min = default(decimal);
            Lon.Value.Max = default(decimal);
        }


        private bool GetPoints(Component<Point<decimal>> comp, string lonStr)
        {
            var points = lonStr.Split(comp.Contains);

            if (points.Length != 2) return false;
            
            if (!GetDecimal(comp, points[0], out var min)) return false;
            
            if (!GetDecimal(comp, points[1], out var max)) return false;

            comp.Value.Max = max;
            comp.Value.Min = min;

            return true;
        }

        /// <summary>
        /// Get decimal value of a Component with remove default start and end characters
        /// </summary>
        /// <typeparam name="T">Component type of extract</typeparam>
        /// <param name="comp">Component extract</param>
        /// <param name="lStr">string value of</param>
        /// <param name="l">extract value </param>
        /// <returns></returns>
        public bool GetDecimal<T>(Component<Point<T>> comp, string lStr, out decimal l) where T : new()
        {
            var number = lStr.Replace(comp.Start, string.Empty)
                .Replace(comp.End, string.Empty)
                .Trim();
            return decimal.TryParse(number,NumberStyles.Any, CultureInfo.CurrentCulture, out l);
        }
    }
}