using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class TransactionGatewayTests
    {
        readonly Random _random = new Random();

        [Test]
        public async Task can_create_find_and_delete_transaction()
        {
            TransactionGateway sut = new TransactionGateway( TestHelpers.ConnectionString );

            string transacDesc = TestHelpers.RandomTestName();
            int transacPrice = _random.Next( 1, 200 );
            int roomieId = 0;
            int collocId = 0;

            Result<int> result = await sut.CreateTransac( transacDesc, transacPrice, roomieId, collocId );
            Assert.That( result.Status, Is.EqualTo( Status.Created ) );

            int transacId = result.Content;

            Result<TransactionData> transacData;
            {
                transacData = await sut.FindById( transacId );
                CheckTransac( transacData, transacDesc, transacPrice, roomieId, collocId );
            }

            {
                transacDesc = TestHelpers.RandomTestName();
                transacPrice = 3;
                roomieId = 1;
                collocId = 1;
                await sut.Update( transacId, transacDesc, transacPrice, roomieId, collocId );

                transacData = await sut.FindById( transacId );
                CheckTransac( transacData, transacDesc, transacPrice, roomieId, collocId );
            }

            {
                Result r = await sut.Delete( transacId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );
                transacData = await sut.FindById( transacId );
                Assert.That( transacData.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        void CheckTransac( Result<TransactionData> t, string transacDesc, int transacPrice, int roomieId, int collocId )
        {
            Assert.That( t.HasError, Is.False );
            Assert.That( t.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( t.Content.TransacDesc, Is.EqualTo( transacDesc ) );
            Assert.That( t.Content.TransacPrice, Is.EqualTo( transacPrice ) );
            Assert.That( t.Content.RoomieId, Is.EqualTo( roomieId ) );
            Assert.That( t.Content.CollocId, Is.EqualTo( collocId ) );
        }
    }
}
