using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Roomies.DAL.Tests
{
    [TestFixture]
    public class TasksGatewayTests
    {
        readonly Random _random = new Random();

        [Test]
        public async Task can_create_find_and_delete_tasks()
        {
            TasksGateway sut = new TasksGateway( TestHelpers.ConnectionString );
            string taskName = TestHelpers.RandomTestName();
            DateTime taskDate = TestHelpers.RandomBirthDate( 1 );
            bool state = true;
            int collocId = 0;
            string taskDes = "hfsdheovqebjbjdfjbjVersion1";

            Result<int> taskResult = await sut.CreateTask( taskName, taskDate, collocId );
            Assert.That( taskResult.Status, Is.EqualTo( Status.Created ) );

            int taskId = taskResult.Content;
            Result<TasksData> task;

            {
                task = await sut.FindByTaskId( taskId );
                CheckTask( task, taskName, taskDate, taskId );
            }

            {
                taskName = TestHelpers.RandomTestName();
                taskDate = TestHelpers.RandomBirthDate( 1 );
                state = false;
                collocId = 1;
                Result r = await sut.Update( taskId, taskName, taskDate, state, collocId, taskDes );
            }

            {
                Result r = await sut.Delete( taskId );
                Assert.That( r.Status, Is.EqualTo( Status.Ok ) );
                task = await sut.FindByTaskId( taskId );
                Assert.That( task.Status, Is.EqualTo( Status.NotFound ) );
            }
        }

        void CheckTask(Result<TasksData> task, string taskName, DateTime taskDate, int taskId )
        {
            Assert.That( task.Status, Is.EqualTo(Status.Ok));
            Assert.That( task.Content.TaskName, Is.EqualTo( taskName ));
            Assert.That( task.Content.TaskDate, Is.EqualTo( taskDate ));
            Assert.That( task.Content.TaskId, Is.EqualTo( taskId ));

        }
    }
}
