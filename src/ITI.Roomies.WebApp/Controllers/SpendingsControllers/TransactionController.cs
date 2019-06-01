using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.TransactionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITI.Roomies.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class TransactionController : Controller
    {
        readonly TransactionGateway _transactionGateway;
        readonly BudgetGateway _budgetGateway;
        public TransactionController( TransactionGateway transactionGateway, BudgetGateway budgetGateway )
        {
            _transactionGateway = transactionGateway;
            _budgetGateway = budgetGateway;
        }

        [HttpPost("createTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionViewModel model )
        {
            int rRoomieId = model.RoomieId;
            if (rRoomieId == 0 )
            {
                BudgetData budget = await _budgetGateway.FindBudgetByCollocIdAndDate( model.CollocId, model.Date );

                rRoomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
                Result<int> result = await _transactionGateway.CreateTransacBudget( model.Price, model.Date, budget.BudgetId, rRoomieId );
                return this.CreateResult( result, o =>
                {
                    o.RouteName = "GetTransacBudgetId";
                    o.RouteValues = transacBudgetId => new { transacBudgetId };
                } );
            }
            else
            {
                int sRoomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
                Result<int> result = await _transactionGateway.CreateTransacDepense( model.Price, model.Date, sRoomieId, rRoomieId );
                return this.CreateResult( result, o =>
                {
                    o.RouteName = "GetTransacDepense";
                    o.RouteValues = transacDepenseId => new { transacDepenseId };
                } );
            }
        }

        #region TransacBudget

        [HttpGet( "getAllTransacBudget" )]
        public async Task<IActionResult> GetAllTransBudget()
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            IEnumerable<TransacBudgetData> result = await _transactionGateway.GetAllTransacBudget( roomieId );
            return Ok( result );
        }

        [HttpGet( "getTransacBudget/{transacBudgetId}", Name="GetTransacBudget" )]
        public async Task<IActionResult> GetTransBudget(int transacBudgetId)
        {
            Result<TransacBudgetData> result = await _transactionGateway.FindTBudgetById( transacBudgetId );
            return this.CreateResult( result );
        }

        [HttpGet( "createTransacBudget" )]
        public async Task<IActionResult> CreateTransacBudget( [FromBody] TransacBudgetViewModel model )
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result = await _transactionGateway.CreateTransacBudget( model.Price, model.Date, model.BudgetId, roomieId );
            return this.CreateResult( result, o =>
             {
                 o.RouteName = "GetTransacBudgetId";
                 o.RouteValues = transacBudgetId => new { transacBudgetId };
             } );
        }



        [HttpPut( "updateTransacBudget/{transacBudgetId}" )]
        public async Task<IActionResult> UpdateTransacBudget( int transacBudget )
        {
            //Result result = await _transactionGateway.UpdateTransacBudget( transacBudget );
            //return this.CreateResult( result );
            throw new NotImplementedException();
        }

        [HttpDelete( "deleteTransacBudget/{transacBudgetId}" )]
        public async Task<IActionResult> DeleteTransacBudget( int transacBudget )
        {
            Result result = await _transactionGateway.DeleteTransacBudget( transacBudget );
            return this.CreateResult( result );
        }
        #endregion

        #region TrasancDepense

        [HttpGet( "GetAllTransacDepense" )]
        public async Task<IActionResult> GetAllTransDepense()
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            IEnumerable<TransacDepenseData> result = await _transactionGateway.GetAllTransacDepense( roomieId );
            return Ok( result );
        }

        [HttpGet( "GetTransacDepense/{transacDepenseId}", Name="GetTransacDepense" )]
        public async Task<IActionResult> getTransacDepense(int transacDepenseId)
        {
            Result<TransacDepenseData> result = await _transactionGateway.FindTDepenseById( transacDepenseId );
            return this.CreateResult( result );
        }

        [HttpGet( "createTransacDenpense" )]
        public async Task<IActionResult> CreateTransacDepense( [FromBody] TransacDepenseViewModel model )
        {
            Result<int> result = await _transactionGateway.CreateTransacDepense( model.Price, model.Date, model.SRoomieId, model.RRoomieId );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetTransacDepense";
                o.RouteValues = transacDepenseId => new { transacDepenseId };
            } );
        }


        [HttpPut( "updateTransacDepense/{transacDepenseId}" )]
        public async Task<IActionResult> UpdateTransacDepense( int transacDepense )
        {
            Result result = await _transactionGateway.UpdateTransacDepense( transacDepense );
            return this.CreateResult( result );
        }

        [HttpDelete("deleteTransactDepense{transacDepenseId}")]
        public async Task<IActionResult> DeleteTransacDepense( int transacDepenseId)
        {
            Result result = await _transactionGateway.DeleteTransacDepense( transacDepenseId );
            return this.CreateResult( result );
        }
        

        #endregion
    }
}
