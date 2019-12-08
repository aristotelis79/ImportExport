using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using ETLCRS.Models.ExportableItems;
using ETLCRS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace ETLCRS.Controllers
{
    /// <summary>
    /// Import Export 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExportController : ControllerBase
    {
        #region fields

        private readonly ITransformationService _transformationService;
        private readonly IExportService<List<string>,string> _exportService;
        private readonly ILogger<ImportExportController> _logger;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transformationService"></param>
        /// <param name="exportService"></param>
        /// <param name="logger"></param>
        public ImportExportController(ITransformationService transformationService,
            IExportService<List<string>, string> exportService,
            ILogger<ImportExportController> logger)
        {
            _transformationService = transformationService ?? throw new ArgumentNullException(nameof(transformationService));
            _exportService = exportService?? throw new ArgumentNullException(nameof(exportService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        #endregion

        #region Actions

        // POST: api/ImportExport
        /// <summary>
        /// Import file
        /// </summary>
        /// <param name="file">import file</param>
        /// <param name="type">select type of result 0 for serialize object  and  1 for txt file representation </param>
        /// <returns>Serialisable object of output response</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Get output transformation of your input file", Description = "Gets input txt file and type of result")]
        public async Task<IActionResult> Import([FromForm] IFormFile file, TypeOfResult type = TypeOfResult.Object)
        {
            if (file == null)
            {
                _logger.Log(LogLevel.Information, "empty file");
                return  new ContentResult(){Content = "empty file" ,StatusCode = 422};
            }

            try
            {
                var transform = await (_transformationService.Transform<Position,List<string>>(file.OpenReadStream()))
                                                                .ConfigureAwait(false);

                switch (type)
                {
                    case TypeOfResult.File:
                        var result = _exportService.Export(transform);
                        return File(Encoding.UTF8.GetBytes(result), "text/plain", $"output_{DateTime.Now}.txt");

                    case TypeOfResult.Object:
                    default:
                        return Ok(JsonConvert.SerializeObject(transform));
                }

            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error,e,"can't import'");

                return  new ContentResult(){Content = "Something going wrong" ,StatusCode = 500};
            }
        }

        #endregion
    }
}