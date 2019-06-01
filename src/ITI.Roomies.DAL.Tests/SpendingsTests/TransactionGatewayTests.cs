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
            ///Need a populated BD pop2
            TransactionGateway sut = new TransactionGateway( TestHelpers.ConnectionString );

            int price = 50;
            DateTime date = TestHelpers.RandomBirthDate( 1 );
            int roomieId = 0;
            int rRoomieId = 1;
            int sRoomieId = 2;
            int budgetId = 0;

            Result<int> tBudgetResult = await sut.CreateTransacBudget( price, date, budgetId, roomieId );
            Assert.That( tBudgetResult.Status, Is.EqualTo( Status.Created ) );

            Result<int> tDepenseResult = await sut.CreateTransacDepense( price, date, sRoomieId, rRoomieId );
            Assert.That( tDepenseResult.Status, Is.EqualTo( Status.Created ) );

            int tBudgetId = tBudgetResult.Content;
            int tDepenseId = tDepenseResult.Content;

            Result<TransacBudgetData> tBudgetData;
            {
                tBudgetData = await sut.FindTBudgetById( tBudgetId );
                CheckTbudget( tBudgetData, tBudgetId, price, date, budgetId, roomieId );
            }
            Result<TransacDepenseData> tDepenseData;
            {
                tDepenseData = await sut.FindTDepenseById( tDepenseId );
                CheckTDepense( tDepenseData, tDepenseId, price, date, sRoomieId, rRoomieId );
            }
            {
                price = 100;
                date = TestHelpers.RandomBirthDate( 2 );
                budgetId = 1;
                roomieId = 1;
                await sut.UpdateTransacBudget( tBudgetId, price, date, budgetId, roomieId );

                tBudgetData = await sut.FindTBudgetById( tBudgetId );
                CheckTbudget( tBudgetData, tBudgetId, price, date, budgetId, roomieId );
            }
            {
                sRoomieId = 0;
                rRoomieId = 2;
                await sut.UpdateTransacDepense( tDepenseId, price, date, sRoomieId, rRoomieId );

                tDepenseData = await sut.FindTDepenseById( tDepenseId );
                CheckTDepense( tDepenseData, tDepenseId, price, date, sRoomieId, rRoomieId );
            }
            {
                Result r = await sut.DeleteTransacBudget( tBudgetId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );

                tBudgetData = await sut.FindTBudgetById( tBudgetId );
                Assert.That( tBudgetData.Status, Is.EqualTo( Status.NotFound ) );
            }
            {
                Result r1 = await sut.DeleteTransacDepense( tDepenseId );
                Assert.That( r1.Status, Is.EqualTo( Status.Ok ) );

                tDepenseData = await sut.FindTDepenseById( tDepenseId );
                Assert.That( tDepenseData.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        private void CheckTDepense( Result<TransacDepenseData> tDepenseData, int tDepenseId, int price, DateTime date, int sRoomieId, int rRoomieId )
        {
            Assert.That( tDepenseData.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( tDepenseData.Content.TDepenseId, Is.EqualTo( tDepenseId ) );
            Assert.That( tDepenseData.Content.Price, Is.EqualTo( price ) );
            Assert.That( tDepenseData.Content.Date, Is.EqualTo( date ) );
            Assert.That( tDepenseData.Content.SRoomieId, Is.EqualTo( sRoomieId ) );
            Assert.That( tDepenseData.Content.RRoomieId, Is.EqualTo( rRoomieId ) );
        }

        private void CheckTbudget( Result<TransacBudgetData> tBudgetData, int tBudgetId, int price, DateTime date, int budgetId, int roomieId )
        {
            Assert.That( tBudgetData.Status, Is.EqualTo( Status.Ok ) );
            Assert.That( tBudgetData.Content.TBudgetId, Is.EqualTo( tBudgetId ) );
            Assert.That( tBudgetData.Content.Price, Is.EqualTo( price ) );
            Assert.That( tBudgetData.Content.Date, Is.EqualTo( date ) );
            Assert.That( tBudgetData.Content.RoomieId, Is.EqualTo( roomieId ) );
        }
    }
    
}
