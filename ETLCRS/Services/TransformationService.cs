using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ETLCRS.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ETLCRS.Services
{
    public class TransformationService : ITransformationService
    {
        #region fields
        
        private readonly ILogger<TransformationService> _logger;
        
        #endregion

        #region ctor

        public TransformationService(ILogger<TransformationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public TOut Transform<TIn,TOut>(Stream stream) where TIn : IExportableItem<TOut>, new()
        {
            CheckForNull(stream);

            var exportableItem = new TIn();
            // batch of lines to process in its loop 
            var lines = new List<string>(exportableItem.NumberOfSubtypes);

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    lines.Add(line);

                    //continue until rich the end of division
                    if (lines.All(string.IsNullOrEmpty) 
                        || !exportableItem.IsExportableDivision(lines)) continue;

                    Processing<TIn, TOut>(exportableItem, lines);

                    //clear batch of lines
                    lines = new List<string>(exportableItem.NumberOfSubtypes);
                }

                //last line
                if (!lines.All(string.IsNullOrEmpty))
                    Processing<TIn, TOut>(exportableItem, lines);
            }

            return exportableItem.ResponseType;
        }


        private void Processing<TIn, TOut>(TIn exportableItem, List<string> lines) where TIn : IExportableItem<TOut>, new()
        {
            var errors = exportableItem.ParseLines(lines);

            if (errors != null && errors.Any())
            {
                _logger.LogInformation(JsonConvert.SerializeObject(errors));
                exportableItem.ClearValues();
                return;
            }

            exportableItem.FeedResponseType(exportableItem);

            exportableItem.ClearValues();
        }


        /// <summary>
        /// Check stream for null
        /// </summary>
        /// <param name="stream">stream data</param>
        private void CheckForNull(Stream stream)
        {
            if (stream != null) return;

            _logger.LogInformation("empty stream");

            throw new ArgumentNullException(nameof(stream));
        }

        #endregion

    }
}