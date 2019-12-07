using System.Collections;

namespace ETLCRS.Services
{
    public interface IExportService<in TIn, out TOut> where TIn : IEnumerable
    {
        /// <summary>
        /// Export type of input Object
        /// </summary>
        /// <param name="items">list of input items</param>
        /// <returns>type of output object</returns>
        TOut Export(TIn items);
    }
}