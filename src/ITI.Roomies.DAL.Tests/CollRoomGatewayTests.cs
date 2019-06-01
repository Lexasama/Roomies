using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class CollRoomGatewayTests
    {
        readonly Random _random = new Random();

        //[Test]
        public async Task can_add_find_and_delete_in_collroom()
        {
            CollRoomGateway sut = new CollRoomGateway( TestHelpers.ConnectionString );

            int collocId = 1;
            int roomieId = 0;

            Result<int> crResult = await sut.AddCollRoom( collocId, roomieId );
            Assert.That( crResult.Status, Is.EqualTo( Status.Created ) );

            Result<CollRoomData> cr;

            {
                cr = await sut.FindById( collocId, roomieId );
                CheckCollRoom( cr, collocId, roomieId );
            }

            {
                Result r = await sut.Delete( collocId, roomieId );
                Assert.That( r.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        void CheckCollRoom( Result<CollRoomData> cr, int collocId, int roomieId)
        {
            Assert.That( cr.Status, Is.EqualTo( Status.Ok));
            Assert.That( cr.Content.CollocId, Is.EqualTo( collocId ) );
            Assert.That( cr.Content.RoomieId, Is.EqualTo( roomieId ) );
        }
    }
}
