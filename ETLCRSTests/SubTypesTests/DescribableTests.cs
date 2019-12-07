using System.Collections.Generic;
using ETLCRS.Models;
using ETLCRS.Models.Subtypes;
using ETLCRSTests.Helpers;
using NUnit.Framework;

namespace ETLCRSTests.SubTypesTests
{
    [TestFixture]
    public class DescribableTests
    {
        private Describable _describable;
        private KeyValuePair<int, SubType> _subtype;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _describable = new Describable();
            _subtype = new KeyValuePair<int, SubType>(1, _describable);
        }

        [Test]
        public void Given_Describable_When_Init_Describable_Item_Are_Init_Successful()
        {
            Assert.That(_describable.Name,Is.Not.Null);
            Assert.That(_describable.Description,Is.Not.Null);
            Assert.That(_describable.Name.Start,Is.EqualTo("#"));
            Assert.That(_describable.Name.End,Is.EqualTo("["));
            Assert.That(_describable.Description.Start,Is.EqualTo("["));
            Assert.That(_describable.Description.End,Is.EqualTo("]"));
        }


        [Test]
        public void Given_Describable_When_Clear_Describable_Values_Clears()
        {
            //arrange

            _describable.Name.Value = "name";
            _describable.Description.Value = "describe";
            //act
            _describable.ClearValues();

            //assert
            Assert.That(_describable.Name.Value, Is.EqualTo(default(string)));
            Assert.That(_describable.Description.Value, Is.EqualTo(default(string)));
        }

        [Test]
        public void Given_Describable_When_Get_If_SubType_Of_Describable_Then_Return_True()
        {
            //arrange
            var line = InputHelpers.Describable_A;
            //act
            var result = _describable.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_Describable_When_Get_If_Not_SubType_Of_Describable_Then_Return_False()
        {
            //arrange
            var line = InputHelpers.Identifiable_A;
            //act
            var result = _describable.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.False);
        }


        [Test]
        public void Given_Describable_When_Feed_Values_From_Line_Then_Feed_Values_And_Not_Error_Return()
        {
            //arrange
            var line = InputHelpers.Describable_B;
            //act
            var result = _describable.FeedValues(line);

            //assert
            Assert.That(result, Is.Null);
            Assert.That(_describable.Name.Value, Is.EqualTo("GCS_Voirol_Unifie_1960"));
            Assert.That(_describable.Description.Value, Is.EqualTo("Voirol Unifie 1960"));
        }


        [Test]
        public void Given_Describable_When_Feed_Values_From_Wrong_Line_Then_Feed_Values_Is_Empty_And_Error_Return()
        {
            //arrange
            _describable.ClearValues();
            var line = "# GCS_Voirol_Unifie_1960[VoirolUnifie1960";
            //act
            var result = _describable.FeedValues(line);

            //assert
            Assert.That(result, Is.EqualTo("# GCS_Voirol_Unifie_1960[VoirolUnifie1960"));
            Assert.That(_describable.Name.Value, Is.Null);
            Assert.That(_describable.Description.Value, Is.Null);
        }
    }
}