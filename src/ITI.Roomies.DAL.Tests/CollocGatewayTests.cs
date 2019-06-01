//using System;
//using System.Threading.Tasks;
//using NUnit.Framework;

//namespace ITI.Roomies.DAL.Tests
//{
//    [TestFixture]
//    public class CollocGatewayTest
//    {
//        readonly Random _random = new Random();

//        [Test]
//        public async Task can_create_find_and_delete_colloc()
//        {
//            CollocGateway sut = new CollocGateway( TestHelpers.ConnectionString );
//            string collocName = TestHelpers.RandomTestName();
//            string collocPic = string.Format("{0}s", Guid.NewGuid());
//            DateTime creationDate = TestHelpers.RandomBirthDate(1);
            

//            //Result<int> collocResult = await sut.CreateColloc( collocName );
//            Assert.That( collocResult.Status, Is.EqualTo( Status.Created ) );

//            int collocId = collocResult.Content;
//            Result<CollocData> colloc;

//            {
//                colloc = await sut.FindById( collocId );
//                CheckColloc( colloc, collocName );
//            }

//            {
//                collocName = TestHelpers.RandomTestName();
//                collocPic = string.Format( "asdvfbgok{0}gh", Guid.NewGuid() );
//                Result r = await sut.Update( collocId, collocPic, collocName );
//            }
//        }

//        void CheckColloc( Result<CollocData> colloc, string collocName )
//        {
//            Assert.That(colloc.Status, Is.EqualTo(Status.Ok));
//            Assert.That( colloc.Content.CollocName, Is.EqualTo( collocName ) );
//        }
        
//    }
//}
