using System;
using System.Collections.Generic;
using System.Globalization;
using ETLCRS.Models.ExportableItems;
using ETLCRS.Services;
using ETLCRSTests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ETLCRSTests
{
    [TestFixture]
    public class TransformationServiceTests
    {
        private Mock<ILogger<TransformationService>> _loggerMock;

        private ITransformationService _service;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _loggerMock = new Mock<ILogger<TransformationService>>();

            _service = new TransformationService(_loggerMock.Object);

            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [Test]
        public void TestForTest()
        {
            Assert.Pass();
        }


        [Test]
        public void Get_Init_TransformationService_Without_ILogger_It_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => { _service = new TransformationService(null); });
        }

        [Test]
        public void Get_Stream_When_Is_Right_And_Produce_Response()
        {
            //arrange
            var stream = $"{InputHelpers.Inputs[0]}{Environment.NewLine}{Environment.NewLine}{InputHelpers.Inputs[1]}".GenerateStreamFromString();
            var output = new List<string> {InputHelpers.Outputs[0], InputHelpers.Outputs[1]};
            
            //act
            var result  = _service.Transform<Position, List<string>>(stream);
            
            //assert
            Assert.That(result,Is.EquivalentTo(output));
        }

        [Test]
        public void Get_Stream_When_Is_Right_Without_Status_Line_And_Produce_Response()
        {
            //arrange
            var stream = $"{InputHelpers.Inputs[1]}{Environment.NewLine}{Environment.NewLine}{InputHelpers.Inputs[2]}".GenerateStreamFromString();
            var output = new List<string> {InputHelpers.Outputs[1], InputHelpers.Outputs[2]};
            
            //act
            var result  = _service.Transform<Position, List<string>>(stream);
            
            //assert
            Assert.That(result,Is.EquivalentTo(output));
        }


        [Test]
        public void Get_Stream_When_Has_Empty_Lines_At_Begin_And_The_End_Of_File_And_Produce_Response()
        {
            //arrange
            var stream = ($"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}" +
                         $"{InputHelpers.Inputs[0]}{Environment.NewLine}{Environment.NewLine}{InputHelpers.Inputs[1]}" +
                         $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}").GenerateStreamFromString();

            var output = new List<string> {InputHelpers.Outputs[0], InputHelpers.Outputs[1]};
            
            //act
            var result  = _service.Transform<Position, List<string>>(stream);
            
            //assert
            Assert.That(result,Is.EquivalentTo(output));
        }


        [Test]
        public void Get_Stream_When_Is_Wrong_One_Line_And_Produce_Only_Right_Lines_Response_And_Log_Errors()
        {
            //arrange
            var wrongInput = InputHelpers.Inputs[0].Replace("lat:", "latt:");
            var stream = $"{wrongInput}{Environment.NewLine}{Environment.NewLine}{InputHelpers.Inputs[1]}".GenerateStreamFromString();
            var output = new List<string> {InputHelpers.Outputs[1]};
            
            //act
            var result  = _service.Transform<Position, List<string>>(stream);

            //assert
            _loggerMock.VerifyLog(LogLevel.Information, "[\"# area: (latt: 31.99, 37.14) - (lon: -2.95, 9.09) [Algeria - north of 32~N]\"," +
                                                        "\"# DEPRECATED: new code = 104025\",\"<4812> +proj=lonlat +a=6378249.145 +rf=293.465 +pm=paris" +
                                                        " +towgs84=-123.0,-206.0,219.0 +no_defs <>\"]", Times.Once());

            Assert.That(result, Is.EquivalentTo(output));
        }

        [Test]
        public void Get_Stream_When_Is_Wrong_One_Line_At_Begin_And_Produce_Only_Right_Lines_Response_And_Log_Errors()
        {
            //arrange
            var wrongInput = InputHelpers.Inputs[0].Replace("# area: ", "areea:");
            var stream = $"{wrongInput}{Environment.NewLine}{Environment.NewLine}{InputHelpers.Inputs[1]}".GenerateStreamFromString();
            var output = new List<string> {InputHelpers.Outputs[1]};
            
            //act
            var result  = _service.Transform<Position, List<string>>(stream);

            //assert
            _loggerMock.VerifyLog(LogLevel.Information, "[\"areea:(lat: 31.99, 37.14) - (lon: -2.95, 9.09) [Algeria - north of 32~N]\"]", Times.Once());

            Assert.That(result, Is.EquivalentTo(output));
        }
    }
}