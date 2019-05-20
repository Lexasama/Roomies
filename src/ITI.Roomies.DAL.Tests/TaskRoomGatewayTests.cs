using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class TaskRoomGatewayTests
    {
        readonly Random _random = new Random();
        
        [Test]
        public async Task can_find_create_and_delete_in_TaskRoom()
        {
            TaskRoomGateway sut = new TaskRoomGateway( TestHelpers.ConnectionString );

            int taskId = 0;
            int roomieId = 1;

            Result<int> trResult = await sut.AddTaskRoom( taskId, roomieId );
            Assert.That( trResult.Status, Is.EqualTo( Status.Created ) );

            Result<TaskRoomData> tr;

            {
                tr = await sut.FindById( taskId );
                CheckTaskRoom( tr, taskId, roomieId );
            }

            {
                Result r = await sut.Delete( taskId, roomieId );
                Assert.That( r.Status, Is.EqualTo( Status.NotFound ) );
            }

        }

        void CheckTaskRoom( Result<TaskRoomData> tr, int taskId, int roomieId )
        {
            Assert.That( tr.Status, Is.EqualTo( Status.Ok ) );
            Assert.That(tr.Content.TaskId, Is.EqualTo( taskId) );
            Assert.That( tr.Content.RoomieId, Is.EqualTo( roomieId));                 
        }
    }
}
