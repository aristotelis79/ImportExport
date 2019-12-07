using System.Collections.Generic;
using System.Linq;

namespace ETLCRS.Models
{
    public abstract class SubType : ComponentSymbols
    {
        /// <summary>
        /// Where is the line in the batch of lines
        /// </summary>
        /// <returns>Potential numbers of order of type</returns>
        internal abstract List<int> NumberOfLine { get; }

        /// <summary>
        /// Feed values of object from line
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public abstract string FeedValues(string line);

        /// <summary>
        /// Clear Component Values
        /// </summary>
        public abstract void ClearValues();

        /// <summary>
        /// Check if is line is type of implement object
        /// </summary>
        /// <param name="subtype">Implement of this interface</param>
        /// <param name="line">line of string</param>
        /// <returns></returns>
        public virtual bool GetSubType(KeyValuePair<int, SubType> subtype, string line) => NumberOfLine.Contains(subtype.Key) 
                                                                                                    && (string.IsNullOrWhiteSpace(this.Start) || line.StartsWith(this.Start))
                                                                                                    && (string.IsNullOrWhiteSpace(this.Contains) || line.Contains(this.Contains))
                                                                                                    && (string.IsNullOrWhiteSpace(this.End) || line.EndsWith(this.End));

        /// <summary>
        /// Get string value of a Component with remove default start and end characters
        /// </summary>
        /// <typeparam name="T">Component type of extract</typeparam>
        /// <param name="comp">Component string of extract</param>
        /// <param name="value">extract value </param>
        /// <returns></returns>
        public static string GetValue<T>(Component<T> comp, string value)
        {
            if (comp.Start != null) value = value.Replace(comp.Start, string.Empty);
            if (comp.End != null) value = value.Replace(comp.End, string.Empty);

            return value.Trim();
        }

        /// <summary>
        ///  Validate if split of line is right
        /// </summary>
        /// <param name="values"></param>
        /// <returns>validate result</returns>
        public virtual bool ValidateSplit(string[] values)
        {
            return values.Length == 3 
                   && values.Where(string.IsNullOrWhiteSpace).Count() <= 1;
        }
    }
}