using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ETLCRS.Models;

namespace ETLCRS.Services
{
    public interface ITransformationService
    {
        /// <summary>
        /// Transform Input to output type
        /// </summary>
        /// <typeparam name="TIn">Type of IExportableItem item</typeparam>
        /// <typeparam name="TOut">type of output item</typeparam>
        /// <param name="stream">Stream of data</param>
        /// <returns></returns>
        /// 
        Task<TOut> Transform<TIn,TOut>(Stream stream) where TIn : IExportableItem<TOut>, new();
    }
}