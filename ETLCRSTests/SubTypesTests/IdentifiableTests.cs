using System.Collections.Generic;
using ETLCRS.Models;
using ETLCRS.Models.Subtypes;
using ETLCRSTests.Helpers;
using NUnit.Framework;

namespace ETLCRSTests.SubTypesTests
{
    [TestFixture]
    public class IdentifiableTests
    {
        private Identifiable _identifiable;
        private KeyValuePair<int, SubType> _subtype;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _identifiable = new Identifiable();
            _subtype = new KeyValuePair<int, SubType>(4, _identifiable);
        }

        [Test]
        public void Given_Identifiable_When_Init_Identifiable_Item_Are_Init_Successful()
        {
            Assert.That(_identifiable.Identity,Is.Not.Null);
            Assert.That(_identifiable.ProjValue,Is.Not.Null);
            Assert.That(_identifiable.Identity.Start,Is.EqualTo("<"));
            Assert.That(_identifiable.Identity.End,Is.EqualTo("+proj"));
            Assert.That(_identifiable.ProjValue.Start,Is.EqualTo("+proj"));
            Assert.That(_identifiable.ProjValue.End,Is.EqualTo("<>"));
        }


        [Test]
        public void Given_Identifiable_When_Clear_Identifiable_Values_Clears()
        {
            //arrange

            _identifiable.Identity.Value = 100;
            _identifiable.ProjValue.Value = "proj 100 200";
            //act
            _identifiable.ClearValues();

            //assert
            Assert.That(_identifiable.Identity.Value, Is.EqualTo(default(int)));
            Assert.That(_identifiable.ProjValue.Value, Is.EqualTo(default(string)));
        }

        [Test]
        public void Given_Identifiable_When_Get_If_SubType_Of_Identifiable_Then_Return_True()
        {
            //arrange
            var line = InputHelpers.Identifiable_A;
            //act
            var result = _identifiable.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Given_Identifiable_When_Get_If_Not_SubType_Of_Identifiable_Then_Return_False()
        {
            //arrange
            var line = InputHelpers.Describable_A;
            //act
            var result = _identifiable.GetSubType(_subtype, line);

            //assert
            Assert.That(result, Is.False);
        }


        [Test]
        public void Given_Identifiable_When_Feed_Values_From_Line_Then_Feed_Values_And_Not_Error_Return()
        {
            //arrange
            var line = InputHelpers.Identifiable_A;
            //act
            var result = _identifiable.FeedValues(line);

            //assert
            Assert.That(result, Is.Null);
            Assert.That(_identifiable.Identity.Value, Is.EqualTo(104000));
            Assert.That(_identifiable.ProjValue.Value, Is.EqualTo("+proj=lonlat +datum=NAD27 +no_defs"));
        }


        [Test]
        public void Given_Identifiable_When_Feed_Values_From_Wrong_Line_Then_Feed_Values_Is_Empty_And_Error_Return()
        {
            //arrange
            _identifiable.ClearValues();
            var line = "<104000 +proj=lonlat +datum=NAD27 +no_defs <>";
            //act
            var result = _identifiable.FeedValues(line);

            //assert
            Assert.That(result, Is.EqualTo("<104000 +proj=lonlat +datum=NAD27 +no_defs <>"));
            Assert.That(_identifiable.Identity.Value, Is.Zero);
            Assert.That(_identifiable.ProjValue.Value, Is.Null);
        }
    }
}