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

        // Renvoie toutes les tâches liées à une collocation
        [HttpGet( "getByCollocId/{id}" )]
        public async Task<IActionResult> GetTasksByCollocIdAsync( int id )
        {
            IEnumerable<TasksData> result = await _tasksGateway.FindTaskByCollocId( id );
            return this.Ok( result );
        }

        // Renvoie toutes les tâches liées à un Roomie
        [HttpGet( "getByRoomieId" )]
        public async Task<IActionResult> GetTasksByRoomieIdAsync()
        {
            int userId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            IEnumerable<TasksData> result = await _tasksGateway.FindTaskByRoomieId( userId );
            return this.Ok( result );
        }


        // Renvoie la tâche correspondant à l'id de la tâche
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
            Result<int> result = await _tasksGateway.CreateTask( model.TaskName, model.TaskDes, model.TaskDate, model.collocId );

            // Si aucune erreur d'exécution, ajoute la tâches avec les roomies à tiTaskRoom
            if( !result.HasError )
            {
                for( int i = 0; i < model.roomiesId.Length; i++ )
                {
                    await _taskRoomGateway.AddTaskRoom( result.Content, model.roomiesId[i] );
                }
            }

            // TO DO : mettre le bon return
            return Ok( 0 );
        }

        // Met à jour l'état de la tâche renseignée
        [HttpPost( "updateTaskState/{id}/{state}" )]
        public async Task<IActionResult> updateTaskStateAsync( int id, bool state )
        {
            Result result = await _tasksGateway.UpdateTaskState( id, state );
            return this.Ok( result );
        }

        // Mise à joru d'une tâche
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



        // Suppression d'un tâche depuis son id
        // TO DO : changer quand la procédure sera correcte
        [HttpDelete( "deleteTask/{taskId}" )]
        public async Task<IActionResult> deleteTaskByIdAsync( int taskId )
        {
            Result result = await _taskRoomGateway.DeleteTaskRoomByTaskId( taskId );

            // Si aucune erreur d'exécution, supprime la tâche de rm.tTasks
            if( !result.HasError )
            {
                await _tasksGateway.DeleteTaskById( taskId );
            }

            // TO DO : mettre le bon return
            return Ok( 0 );
        }
    }
}
