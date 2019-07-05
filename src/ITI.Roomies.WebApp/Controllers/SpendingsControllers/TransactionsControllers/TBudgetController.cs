using ITI.Roomies.DAL;
using ITI.Roomies.DAL.Spendings.TransactionsGateways;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.TransactionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITI.Roomies.WebApp.Controllers.SpendingsControllers.TransactionsControllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class TBudgetController : Controller
    {
        readonly TBudgetGateway _tBudgetGateway;

        public TBudgetController( TBudgetGateway tBudgetGateway )
        {
            _tBudgetGateway = tBudgetGateway;
        }

        [HttpGet( "getAllTBudget" )]
        public async Task<IActionResult> GetAllTransBudget()
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            IEnumerable<TransacBudgetData> result = await _tBudgetGateway.GetAllTransacBudget( roomieId );
            return Ok( result );
        }

        [HttpGet( "getAllDepense/{budgetId}" )]
        public async Task<IActionResult> GetDepenses(int budgetId)
        {
            var depenses = 0;
            IEnumerable<TransacBudgetData> expensesList = await _tBudgetGateway.GetDepensesOfBudget( budgetId );
            foreach(TransacBudgetData transac in expensesList )
            {
                depenses = depenses + transac.Price;
            }
            
            return Ok( expensesList );
        }

        [HttpGet( "getTBudget/{tBudgetId}", Name = "GetTransacBudget" )]
        public async Task<IActionResult> GetTransBudget( int transacBudgetId )
        {
            Result<TransacBudgetData> result = await _tBudgetGateway.FindTBudgetById( transacBudgetId );
            return this.CreateResult( result );
        }

        [HttpPost( "createTBudget" )]
        public async Task<IActionResult> CreateTBudget( [FromBody]TransacBudgetViewModel model )
        {
            int RoomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result = await _tBudgetGateway.CreateTransacBudget( model.Price, model.Date, model.CategoryId, RoomieId );
            return Ok( result );

        }

        [HttpPut( "updateTBudget/{tBudget}" )]
        public async Task<IActionResult> UpdateTransacBudget( [FromBody] TransacBudgetData model )
        {
            Result result = await _tBudgetGateway.UpdateTransacBudget( model.TBudgetId, model.Price, model.Date, model.BudgetId, model.RoomieId );
            return this.CreateResult( result );
        }

        [HttpDelete( "deleteTBudget/{tBudgetId}" )]
        public async Task<IActionResult> DeleteTransacBudget( int tBudgetId )
        {
            Result result = await _tBudgetGateway.DeleteTransacBudget( tBudgetId );
            return this.CreateResult( result );
        }

    }
}
