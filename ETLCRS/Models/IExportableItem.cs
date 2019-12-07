using System.Collections.Generic;
using System.Globalization;

namespace ETLCRS.Models
{
    public interface IExportableItem<T>
    {
        /// <summary>
        /// Number of subtype items
        /// </summary>
        int NumberOfSubtypes { get; }

        /// <summary>
        /// Response Type of transformation
        /// </summary>
        T ResponseType{ get; }

        /// <summary>
        /// Check if is the end of Batch of subtypes
        /// </summary>
        /// <param name="lines">the lines of string in the batch</param>
        /// <returns>If it is the end of one batch</returns>
        bool IsExportableDivision(List<string> lines);

        /// <summary>
        /// Parse input batch of line and extract proper values to subtypes 
        /// </summary>
        /// <param name="line">Input batch of lines</param>
        /// <returns>Errors line who can not parse properly</returns>
        List<string> ParseLines(List<string> line);

        /// <summary>
        /// Feel response object with values of parse
        /// </summary>
        /// <param name="exportableItem"></param>
        void FeedResponseType(IExportableItem<T> exportableItem);

        /// <summary>
        /// Clear parsed subtypes values
        /// </summary>
        void ClearValues();
    }
}