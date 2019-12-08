using System.Collections.Generic;
using System.Globalization;
using ETLCRS.Models;
using ETLCRS.Models.Subtypes;
using ETLCRSTests.Helpers;
using NUnit.Framework;

namespace ETLCRSTests.SubTypesTests
{
    [TestFixture]
    public class AreaTests
    {
        private Area _area;
        private KeyValuePair<int, SubType> _subtype;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            _area = new Area();
            _subtype = new KeyValuePair<int, SubType>(2, _area);
        }

        [Test]
        public void Given_Area_When_Init_Area_Item_Are_Init_Successful()
        {
            Assert.That(_area.Lat,Is.Not.Null);
            Assert.That(_area.Lon,Is.Not.Null);
            Assert.That(_area.Lat.Value,Is.Not.Null);
            Assert.That(_area.Lon.Value,Is.Not.Null);
            Assert.That(_area.Start,Is.EqualTo("#"));
            Assert.That(_area.Contains,Is.EqualTo("area:"));
            Assert.That(_area.End,Is.EqualTo("]"));
            Assert.That(_area.Lat.Start,Is.EqualTo("(lat:"));
            Assert.That(_area.Lat.Contains,Is.EqualTo(","));
            Assert.That(_area.Lat.End,Is.EqualTo(")"));
            Assert.That(_area.Lon.Start,Is.EqualTo("(lon:"));
            Assert.That(_area.Lon.Contains,Is.EqualTo(","));
            Assert.That(_area.Lon.End,Is.EqualTo(")"));
        }


        [Test]
        public void Given_Area_When_Clear_Area_Values_Clears()
        {
            //arrange
            _area.Lat.Value.Min = 10;
            _area.Lat.Value.Max = 20;
            _area.Lon.Value.Min = -30;
            _area.Lon.Value.Max = -20;

            //act
            _area.ClearValues();

            //assert
            Assert.That(_area.Lat.Value.Min, Is.EqualTo(default(decimal)));
            Assert.That(_area.Lat.Value.Max, Is.EqualTo(default(decimal)));
            Assert.That(_area.Lon.Value.Min, Is.EqualTo(default(decimal)));
            Assert.That(_area.Lon.Value.Max, Is.EqualTo(default(decimal)));
        }

        [Test]
        public void Given_Area_When_Get_If_SubType_Of_Area_Then_Return_True()
        {
            //arrange
            var line = InputHelpers.Area_A;
            //act
            var result = _area.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_Area_When_Get_If_Not_SubType_Of_Area_Then_Return_False()
        {
            //arrange
            var line = InputHelpers.Describable_A;
            //act
            var result = _area.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.False);
        }


        [Test]
        public void Given_Area_When_Feed_Values_From_Line_Then_Feed_Values_And_Not_Error_Return()
        {
            //arrange
            var line = InputHelpers.Area_B;
            //act
            var result = _area.FeedValues(line);

            //assert
            Assert.That(result, Is.Null);
            Assert.That(_area.Lat.Value.Min, Is.EqualTo(31.99));
            Assert.That(_area.Lat.Value.Max, Is.EqualTo(37.14));
            Assert.That(_area.Lon.Value.Min, Is.EqualTo(-2.95));
            Assert.That(_area.Lon.Value.Max, Is.EqualTo(9.09));
        }


        [Test]
        public void Given_Area_When_Feed_Values_From_Wrong_Line_Then_Feed_Values_Is_Empty_And_Error_Return()
        {
            //arrange
            _area.ClearValues();
            var line = "# area: (lat: ,) - (lon: -2.95, 9.09) [Algeria - north of 32~N]";
            //act
            var result = _area.FeedValues(line);

            //assert
            Assert.That(result, Is.EqualTo("# area: (lat: ,) - (lon: -2.95, 9.09) [Algeria - north of 32~N]"));
            Assert.That(_area.Lat.Value.Min, Is.Zero);
            Assert.That(_area.Lat.Value.Max, Is.Zero);
            Assert.That(_area.Lon.Value.Min, Is.Zero);
            Assert.That(_area.Lon.Value.Max, Is.Zero);
        }
    }
}