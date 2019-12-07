using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ETLCRS.Services
{
    public class StringExportService : IExportService<List<string>,string>
    {
        private readonly ILogger<StringExportService> _logger;

        public StringExportService(ILogger<StringExportService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public string Export(List<string> items)
        {
            CheckForNull(items);

            var sb = new StringBuilder();
            foreach (var p in items)
            {
                sb.Append(p);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Check items for null
        /// </summary>
        /// <typeparam name="T">type of input object</typeparam>
        /// <param name="items">list of input items</param>
        private void CheckForNull<T>(List<T> items)
        {
            if (items != null) return;

            _logger.Log(LogLevel.Information,"empty file");

            throw new ArgumentNullException(nameof(items));
        }
    }
}