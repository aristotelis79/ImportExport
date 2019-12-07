using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ETLCRS.Models.ExportableItems;
using ETLCRSTests.Helpers;
using NUnit.Framework;

namespace ETLCRSTests
{
    [TestFixture]
    public class ExportableItemsTests
    {
        private Position _position;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            _position = new Position();
        }



        [Test]
        public void Given_Position_When_Init_Position_Exportable_Item_All_Subtypes_Are_Init_Successful()
        {
            Assert.That(_position.Identifiable, Is.Not.Null);
            Assert.That(_position.Describable, Is.Not.Null);
            Assert.That(_position.Area, Is.Not.Null);
            Assert.That(_position.StatusSubType, Is.Not.Null);
            Assert.That(Position.TypeOfLines.ContainsValue(_position.Identifiable));
            Assert.That(Position.TypeOfLines.ContainsValue(_position.Describable));
            Assert.That(Position.TypeOfLines.ContainsValue(_position.Area));
            Assert.That(Position.TypeOfLines.ContainsValue(_position.StatusSubType));
            Assert.That(_position.NumberOfSubtypes.Equals(4));
        }

        [Test]
        public void Given_Position_When_Clear_Position_All_Subtypes_Values_Clears()
        {
            //arrange
            _position.Identifiable.Identity.Value = 1;
            _position.Identifiable.ProjValue.Value = "some proj";
            _position.Area.Lat.Value.Min = 1;
            _position.Area.Lat.Value.Max = 2;
            _position.Area.Lon.Value.Min = 3;
            _position.Area.Lon.Value.Max = 4;
            _position.Describable.Name.Value = "name";
            _position.Describable.Description.Value = "describe";
            _position.StatusSubType.Status.Value = "status";
            //act
            _position.ClearValues();

            //assert
            Assert.That(_position.Identifiable.Identity.Value, Is.EqualTo(default(int)));
            Assert.That(_position.Identifiable.ProjValue.Value, Is.EqualTo(default(string)));
            Assert.That(_position.Area.Lat.Value.Min, Is.EqualTo(default(decimal)));
            Assert.That(_position.Area.Lat.Value.Max, Is.EqualTo(default(decimal)));
            Assert.That(_position.Area.Lon.Value.Min, Is.EqualTo(default(decimal)));
            Assert.That(_position.Area.Lon.Value.Max, Is.EqualTo(default(decimal)));
            Assert.That(_position.Describable.Name.Value, Is.EqualTo(default(string)));
            Assert.That(_position.Describable.Description.Value, Is.EqualTo(default(string)));
            Assert.That(_position.StatusSubType.Status.Value, Is.EqualTo(default(string)));
        }

        [Test]
        public void Given_Position_When_FeedPositionType_Then_Right_Response_Type_Contains()
        {
            //arrange
            _position.ClearValues();

            _position.Identifiable.Identity.Value = 1;
            _position.Identifiable.ProjValue.Value = "some proj";
            _position.Area.Lat.Value.Min = 1;
            _position.Area.Lat.Value.Max = 2;
            _position.Area.Lon.Value.Min = 3;
            _position.Area.Lon.Value.Max = 4;
            _position.Describable.Name.Value = "name";
            _position.Describable.Description.Value = "describe";
            _position.StatusSubType.Status.Value = "status";
            
            //act
            _position.FeedResponseType(_position);

            //assert
            Assert.That(_position.ResponseType.Contains("1 name describe 1 2 3 4 some proj status"));
        }

        [Test]
        public void Given_Position_Without_Status_When_FeedPositionType_Then_Right_Response_Type_Contains()
        {
            //arrange
            _position.ClearValues();

            _position.Identifiable.Identity.Value = 1;
            _position.Identifiable.ProjValue.Value = "some proj";
            _position.Area.Lat.Value.Min = 1;
            _position.Area.Lat.Value.Max = 2;
            _position.Area.Lon.Value.Min = 3;
            _position.Area.Lon.Value.Max = 4;
            _position.Describable.Name.Value = "name";
            _position.Describable.Description.Value = "describe";
            
            //act
            _position.FeedResponseType(_position);

            //assert
            Assert.That(_position.ResponseType.Contains("1 name describe 1 2 3 4 some proj"));
        }

        
        [Test]
        public void Given_Position_When_Division_Occur_Then_Return_True()
        {
            //arrange
            var lines = new List<string>()
            {
                InputHelpers.Describable_A ,
                InputHelpers.Area_A,
                InputHelpers.Identifiable_A,
                InputHelpers.StatusSubType_A,
                $"{Environment.NewLine}"
            };
            
            //act
            var isDivisionType = _position.IsExportableDivision(lines);

            //assert
            Assert.That(isDivisionType,Is.True);
        }

        [Test]
        public void Given_Lines_With_Empty_Lines_At_End_When_Division_Occur_Then_Return_True()
        {
            //arrange
            var lines = new List<string>()
            {
                InputHelpers.Describable_A ,
                InputHelpers.Area_A,
                InputHelpers.Identifiable_A,
                InputHelpers.StatusSubType_A,
                $"{Environment.NewLine}",
                $"{Environment.NewLine}",
                $"{Environment.NewLine}",
                $"{Environment.NewLine}",
                $"{Environment.NewLine}"

            };
            
            //act
            var isDivisionType = _position.IsExportableDivision(lines);

            //assert
            Assert.That(isDivisionType,Is.True);
        }

        [Test]
        public void Given_Lines_With_Empty_Lines_At_Start_When_Division_Occur_Then_Return_True()
        {
            //arrange
            var lines = new List<string>()
            {
                $"{Environment.NewLine}",
                $"{Environment.NewLine}",
                $"{Environment.NewLine}",
                $"{Environment.NewLine}",
                InputHelpers.Describable_A ,
                InputHelpers.Area_A,
                InputHelpers.Identifiable_A,
                InputHelpers.StatusSubType_A,
                $"{Environment.NewLine}"

            };
            
            //act
            var isDivisionType = _position.IsExportableDivision(lines);

            //assert
            Assert.That(isDivisionType,Is.True);
        }


        [Test]
        public void Given_Lines_With_Empty_Lines_At_Middle_When_Division_Occur_Then_Return_True()
        {
            //arrange
            var lines = new List<string>()
            {
                InputHelpers.Describable_A ,
                InputHelpers.Area_A,
                $"{Environment.NewLine}",
                InputHelpers.Identifiable_A,
                InputHelpers.StatusSubType_A,
                $"{Environment.NewLine}"

            };
            
            //act
            var isDivisionType = _position.IsExportableDivision(lines);

            //assert
            Assert.That(isDivisionType,Is.True);
        }

        [Test]
        public void Given_Lines_When_Parse_Lines_Then_Parse_Values_And_No_Errors_Returns()
        {
            //arrange
            var lines = new List<string>()
            {
                InputHelpers.Describable_A ,
                InputHelpers.Area_A,
                InputHelpers.Identifiable_A,
                InputHelpers.StatusSubType_A,
                $"{Environment.NewLine}"

            };
            
            //act
            var errors = _position.ParseLines(lines);

            //assert
            Assert.That(errors,Is.Empty);
            Assert.That(_position.ToString(),Is.EqualTo("104000 GCS_Assumed_Geographic_1 NAD27 for shapefiles w/o a PRJ -90.0 90.0 -180.0 180.0 +proj=lonlat +datum=NAD27 +no_defs DISCONTINUED"));
        }

        [Test]
        public void Given_Lines_When_Parse_Lines_Without_Status_Then_Parse_Values_And_No_Errors_Returns()
        {
            //arrange
            _position.ClearValues();

            var lines = new List<string>()
            {
                InputHelpers.Describable_B ,
                InputHelpers.Area_B,
                InputHelpers.Identifiable_B,
                $"{Environment.NewLine}"

            };
            
            //act
            var errors = _position.ParseLines(lines);

            //assert
            Assert.That(errors,Is.Empty);
            Assert.That(_position.ToString(),Is.EqualTo("4305 GCS_Voirol_Unifie_1960 Voirol Unifie 1960 31.99 37.14 -2.95 9.09 +proj=lonlat +a=6378249.145 +rf=293.465 +towgs84=-123.0,-206.0,219.0 +no_defs"));
        }

        [Test]
        public void Given_Lines_When_Parse_Lines_With_Error_Then__Parse_Values_And_Errors_Returns()
        {
            //arrange
            _position.ClearValues();

            var lines = new List<string>()
            {
                InputHelpers.Describable_B ,
                InputHelpers.Area_B,
                "Some wrong",
                //InputHelpers.Identifiable_B,
                $"{Environment.NewLine}"

            };
            
            //act
            var errors = _position.ParseLines(lines);

            //assert
            Assert.That(errors,Is.EqualTo(new List<string>{"Some wrong"}));
        }
    }
}