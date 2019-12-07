using System.Collections.Generic;
using ETLCRS.Models;
using ETLCRS.Models.Subtypes;
using ETLCRSTests.Helpers;
using NUnit.Framework;

namespace ETLCRSTests.SubTypesTests
{

    [TestFixture]
    public class StatusSubTypeTests
    {
        private StatusSubType _statusSubType;
        private KeyValuePair<int, SubType> _subtype;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _statusSubType = new StatusSubType();
            _subtype = new KeyValuePair<int, SubType>(3, _statusSubType);
        }

        [Test]
        public void Given_StatusSubType_When_Init_StatusSubType_Item_Are_Init_Successful()
        {
            Assert.That(_statusSubType.Status ,Is.Not.Null);
            Assert.That(_statusSubType.Start,Is.EqualTo("#"));
        }


        [Test]
        public void Given_StatusSubType_When_Clear_StatusSubType_Values_Clears()
        {
            //arrange

            _statusSubType.Status.Value = "Status";

            //act
            _statusSubType.ClearValues();

            //assert
            Assert.That(_statusSubType.Status.Value, Is.EqualTo(default(string)));
        }

        [Test]
        public void Given_StatusSubType_When_Get_If_SubType_Of_StatusSubType_Then_Return_True()
        {
            //arrange
            var line = InputHelpers.StatusSubType_A;
            //act
            var result = _statusSubType.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_StatusSubType_When_Get_If_Not_SubType_Of_StatusSubType_Then_Return_False()
        {
            //arrange
            var line = InputHelpers.Describable_A;
            //act
            var result = _statusSubType.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.False);
        }


        [Test]
        public void Given_StatusSubType_When_Feed_Values_From_Line_Then_Feed_Values_And_Not_Error_Return()
        {
            //arrange
            var line = InputHelpers.StatusSubType_B;
            //act
            var result = _statusSubType.FeedValues(line);

            //assert
            Assert.That(result, Is.Null);
            Assert.That(_statusSubType.Status.Value, Is.EqualTo("DEPRECATED"));
        }


        [Test]
        public void Given_Describable_When_Feed_Values_From_Wrong_Line_Then_Feed_Values_Is_Empty_And_Error_Return()
        {
            //arrange
            var line = "#DEPRECATED";
            //act
            var result = _statusSubType.FeedValues(line);

            //assert
            Assert.That(result, Is.EqualTo("#DEPRECATED"));
            Assert.That(_statusSubType.Status.Value, Is.Null);
        }
    }
}