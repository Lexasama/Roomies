using ITI.Roomies.DAL;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.SpendingsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ITI.Roomies.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    [Authorize( AuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme )]
    public class BudgetController : Controller
    {
        readonly BudgetGateway _budgetGateway;

        public BudgetController( BudgetGateway budgetGateway)
        {
            _budgetGateway = budgetGateway;
        }

        [HttpGet( "getBudgets/{collocId}")]
        public async Task<IActionResult> getAllBudget(int collocId)
        {
            IEnumerable<BudgetData> budgetDatas = await _budgetGateway.GetAll(collocId);
            return Ok( budgetDatas );
        }

        [HttpGet("getBudgetById/{budgetId}", Name="GetBudget")]
        public async Task<IActionResult> getBudgetById(int budgetId)
        {
            Result<BudgetData> result = await _budgetGateway.FindBudgetById( budgetId );
            return Ok( result );
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBudget( [FromBody] BudgetViewModel model)
        {
            //int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier).Value );
            
            Result<int> result = await _budgetGateway.CreateBudget( model.CategoryId, model.Date1, model.Date2, model.Amount, model.CollocId );
            return this.CreateResult( result, o =>
            {
                o.RouteName = "GetBudget";
                o.RouteValues = budgetId => new { budgetId };
            } );
        }

        [HttpPost( "delete/{budgetId}")]
        public async Task<IActionResult> DeleteBudget (int budgetId)
        {
            Result result = await _budgetGateway.DeleteBudget( budgetId);
            return this.CreateResult( result );
        }

        [HttpPost( "update/{budgetId}" )]
        public async Task<IActionResult> UpdateBudget( int budgetId, [FromBody] BudgetViewModel model)
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result result = await _budgetGateway.UpdateBudget( budgetId, model.CategoryId, model.Date1, model.Date2, model.Amount, model.CollocId );
            return this.CreateResult( result );
        }
    }
}
