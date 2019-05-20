using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class RoomieGatewayTests
    {
        readonly Random _random = new Random();

        [Test]
        public async Task can_create_find_and_delete_roomies()
        {
            RoomiesGateway sut = new RoomiesGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 18, 25 ) );
            string phone = "0000000001";
            string email = string.Format( "roomies{0}@test.com", Guid.NewGuid() );
            string desc = string.Format( "patate{0}djgeofahgrajgran", Guid.NewGuid() );

            //Result<int> roomieResult = await sut.CreateRoomie( firstName, lastName, birthDate, phone, email );
            //Assert.That( roomieResult.Status, Is.EqualTo( Status.Created ) );

            //int roomieId = roomieResult.Content;
            //Result<RoomiesData> roomie;

            //{
            //    roomie = await sut.FindById( roomieId );
            //    CheckRoomie( roomie, firstName, lastName, birthDate, phone, email );
            //}

            //{
            //    desc = string.Format( "azqethryssdsfhj{0}ygiaevsf<hifhpatte", Guid.NewGuid() );
            //    phone = "0000000003";
            //    Result r = await sut.Update( roomieId, desc, phone );
            //    roomie = await sut.FindById( roomieId );
            //    Assert.That( roomie.Status, Is.EqualTo( Status.NotFound ) );
            //}

            //{
            //    Result r = await sut.Delete( roomieId );
            //    Assert.That( r.Status, Is.EqualTo( Status.Ok ) );
            //    roomie = await sut.FindById( roomieId );
            //    Assert.That( roomie.Status, Is.EqualTo( Status.NotFound ) );
            //}
        }

        [Test]
        public async Task can_find_by_email()
        {
            RoomiesGateway sut = new RoomiesGateway( TestHelpers.ConnectionString );
            string firstName = TestHelpers.RandomTestName();
            string lastName = TestHelpers.RandomTestName();
            DateTime birthDate = TestHelpers.RandomBirthDate( _random.Next( 18, 25 ) );
            string phone = "0000000001";
            string email = string.Format( "roomies1{0}@test.com", Guid.NewGuid() );
            string desc = string.Format( "patate{0}djgeofahgrajgran", Guid.NewGuid() );

            //Result<int> roomieResult = await sut.CreateRoomie( firstName, lastName, birthDate, phone, email );
            //Assert.That( roomieResult.Status, Is.EqualTo( Status.Created ) );

            Result<RoomiesData> roomie;

            {
                roomie = await sut.FindByEmail( email );
                CheckRoomie( roomie, firstName, lastName, birthDate, phone, email );
            }
        }

        void CheckRoomie( Result<RoomiesData> roomie, string firstName, string lastName, DateTime birthDate, string phone, string email )
        {
            Assert.That( roomie.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( roomie.Content.FirstName, Is.EqualTo( firstName ) );
            Assert.That( roomie.Content.LastName, Is.EqualTo( lastName ) );
            Assert.That( roomie.Content.BirthDate, Is.EqualTo( birthDate ) );
            Assert.That( roomie.Content.Phone, Is.EqualTo( phone ) );
            Assert.That( roomie.Content.Email, Is.EqualTo( email ) );
        }
    }
}
