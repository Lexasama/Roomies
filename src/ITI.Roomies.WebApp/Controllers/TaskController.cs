using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.TaskModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Roomies.WebApp.Controllers
{

    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class TaskController : Controller
    {
        readonly TasksGateway _tasksGateway;
        readonly TaskRoomGateway _taskRoomGateway;

        public TaskController( TasksGateway tasksGateway, TaskRoomGateway taskRoomGateway )
        {
            _tasksGateway = tasksGateway;
            _taskRoomGateway = taskRoomGateway;
        }

        /// <summary>
        /// Renvoie toutes les tâches liées à une collocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet( "getByCollocId/{id}" )]
        public async Task<IActionResult> GetTasksByCollocIdAsync( int id )
        {
            IEnumerable<TasksData> result = await _tasksGateway.FindTaskByCollocId( id );
            return this.Ok( result );
        }

        /// <summary>
        /// Renvoie toutes les tâches liées à un Roomie
        /// </summary>
        /// <returns></returns>
        [HttpGet( "getByRoomieId" )]
        public async Task<IActionResult> GetTasksByRoomieIdAsync()
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            IEnumerable<TasksData> result = await _tasksGateway.FindTaskByRoomieId( userId );
            return this.Ok( result );
        }

        /// <summary>
        /// Renvoie la tâche correspondant à l'id de la tâche
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet( "getByTaskId/{id}" )]
        public async Task<IActionResult> GetTaskByTaskIdAsync( int id)
        {
            IEnumerable<TasksData> result = await _tasksGateway.FindTaskByTaskId( id );
            // TO DO : mettre le bon return
            return this.Ok( result );
        }

        //[HttpPost]
        //public async Task<int> CreateTask( [FromBody] CollocViewModel model )
        //{
        //Result<int> result = await _collocGateway.CreateColloc( model.CollocName );
        //int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
        //Result<int> result2 = await _collRoomGateway.AddCollRoom( result.Content, userId );

        //return result.Content;

        //}

        // Création de tâches depuis le modèle, ne prend pas en compte la description
        [HttpPost( "createTask" )]
        public async Task<IActionResult> createTaskSansDescAsync( [FromBody] TaskViewModel model )
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result = await _tasksGateway.CreateTask( model.TaskName, model.TaskDes, model.TaskDate, model.collocId, roomieId);

            return this.CreateResult( result);
        }

        /// <summary>
        /// Met à jour l'état de la tâche renseignée
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost( "updateTaskState/{id}/{state}" )]
        public async Task<IActionResult> updateTaskStateAsync( int id, bool state )
        {
            Result result = await _tasksGateway.UpdateTaskState( id, state );
            return this.Ok( result );
        }

        /// <summary>
        /// Mise à jour d'une tâche
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost( "updateTask/{taskId}" )]
        public async Task<IActionResult> createTaskSansDescAsync(int taskId, [FromBody] TaskViewModel model )
        {
            Result result = await _tasksGateway.UpdateTask( taskId, model.TaskName, model.TaskDate, model.TaskDes );

            // Si aucune erreur d'exécution, supprime puis ajoute la tâche avec les roomies à tiTaskRoom
            if( !result.HasError )
            {
                await _taskRoomGateway.DeleteTaskRoomByTaskId( taskId );
                for( int i = 0; i < model.roomiesId.Length; i++ )
                {

                    await _taskRoomGateway.AddTaskRoom( taskId, model.roomiesId[i] );
                }
            }

            // TO DO : mettre le bon return
            return Ok( result );
        }

        /// <summary>
        /// Delete a task unsing it Id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete( "deleteTask/{taskId}" )]
        public async Task<IActionResult> DeleteTaskByIdAsync( int taskId )
        {
            Result result =  await _tasksGateway.DeleteTaskById( taskId );
            return this.CreateResult( result );
        }
    }
}
