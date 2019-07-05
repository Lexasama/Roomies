//using System;
//using System.Threading.Tasks;
//using NUnit.Framework;

//namespace ITI.Roomies.DAL.Tests
//{
//    [TestFixture]
//    public class ItemGatewayTests
//    {
//        readonly Random _random = new Random();

//        [Test]
//        public async Task can_create_find_update_and_delete_an_item()
//        {
//            ItemGateway sut = new ItemGateway( TestHelpers.ConnectionString );

//            int itemPrice = 10;
//            string itemName = TestHelpers.RandomTestName();
//            int courseId = 0;
//            int roomieId = 0;

//            //Result<int> itemResult = await sut.CreateItem( itemPrice, itemName, courseId, roomieId);
//            //Assert.That( itemResult.Status, Is.EqualTo(Status.Created));

//            //int itemId = itemResult.Content;
//            Result<ItemData> item;

//            {
//                item = await sut.FindById( itemId );
//                CheckItem( item, itemPrice, itemName, courseId, roomieId );
//            }

//            {
//                Result r = await sut.Delete( itemId );
//                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );
//                item = await sut.FindById( itemId );
//                Assert.That( item.Status, Is.EqualTo( Status.NotFound ) );
//            }
//        }


//        void CheckItem(Result<ItemData> item, int itemPrice, string itemName, int courseId, int roomieId)
//        {
//            Assert.That( item.Status, Is.EqualTo( Status.Ok ) );
//            Assert.That( item.Content.ItemName, Is.EqualTo( itemName ) );
//            Assert.That( item.Content.ItemPrice, Is.EqualTo( itemPrice ) );
//            Assert.That( item.Content.CourseId, Is.EqualTo( courseId ) );
//            Assert.That( item.Content.RoomieId, Is.EqualTo( roomieId ) );

//        }
//    }
//}
