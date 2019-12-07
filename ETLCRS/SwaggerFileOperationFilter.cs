﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ETLCRS
{
    /// <summary>
    /// Fix Swagger for get file at post
    /// </summary>
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod != "POST")
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            var isFormFileFound = false;

            //try to find IEnumerable<IFormFile> parameters or IFormFile nested in other classes
            foreach (var parameter in operation.Parameters)
            {
                if (parameter is NonBodyParameter nonBodyParameter)
                {
                    var methodParameter =
                    context.ApiDescription.ParameterDescriptions.FirstOrDefault(x => x.Name == parameter.Name);
                    if (methodParameter != null)
                    {
                        if (typeof(IFormFile).IsAssignableFrom(methodParameter.Type))
                        {
                            nonBodyParameter.Type = "file";
                            nonBodyParameter.In = "formData";
                            isFormFileFound = true;
                        }
                        else if (typeof(IEnumerable<IFormFile>).IsAssignableFrom(methodParameter.Type))
                        {
                            nonBodyParameter.Items.Type = "file";
                            nonBodyParameter.In = "formData";
                            isFormFileFound = true;
                        }
                    }
                }
            }



            //try to find IFormFile parameters of method
            var formFileParameters = context.ApiDescription.ActionDescriptor.Parameters
                .Where(x => x.ParameterType == typeof(IFormFile)).ToList();
            foreach (var apiParameterDescription in formFileParameters)
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = apiParameterDescription.Name,
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
            }

            if (formFileParameters.Any())
            {
                foreach (var propertyInfo in typeof(IFormFile).GetProperties())
                {
                    ((List<IParameter>)operation.Parameters).RemoveAll(x => x.Name == propertyInfo.Name);
                }
            }


            if (isFormFileFound)
                operation.Consumes.Add("multipart/form-data");
        }
    }

}