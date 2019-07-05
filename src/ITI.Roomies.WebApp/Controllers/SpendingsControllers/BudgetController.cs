using ITI.Roomies.DAL;
using ITI.Roomies.DAL.Spendings;
using ITI.Roomies.WebApp.Authentication;
using ITI.Roomies.WebApp.Models.SpendingsViewModels;
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
    public class BudgetController : Controller
    {
        readonly BudgetGateway _budgetGateway;

        public BudgetController( BudgetGateway budgetGateway )
        {
            _budgetGateway = budgetGateway;
        }

        [HttpGet( "getBudgets/{collocId}" )]
        public async Task<IActionResult> GetAllBudget( int collocId )
        {
            IEnumerable<BudgetData> budgetDatas = await _budgetGateway.GetAll( collocId );
            return Ok( budgetDatas );
        }

        [HttpGet("getAllBudgetCatData/{collocId}")]
        public async Task<IActionResult> GetAllBudgetCat(int collocId)
        {
            IEnumerable<BudgetCatData> budgetCats = await _budgetGateway.GetAllChartDataByCollocId(collocId);
            return Ok(budgetCats);
        }
        [HttpGet( "currentBudget/{categoryId}" )]
        public async Task<IActionResult> GetCurrentBudget( int categoryId )
        {
            BudgetData currentBudget = await _budgetGateway.GetCurrentBudget( categoryId );
            return Ok( currentBudget);
        }

        [HttpGet("getBudgetByTime/{collocId}/{currentDate}")]
        public async Task<IActionResult> GetByTime(int collocId, string currentDate)
        {
            DateTime dateObj = Convert.ToDateTime( currentDate );
            IEnumerable<BudgetCatData> budgetByTime = await _budgetGateway.GetChartDataByTime(collocId, dateObj);
            return Ok(budgetByTime);

        }

        [HttpGet("getDailyBudget/{collocId}") ]
        public async Task<IActionResult> GetDailyBudget( int collocId )
        {
            DateTime day = DateTime.Today;
            IEnumerable<BudgetCatData> DailyBudget = await _budgetGateway.GetDailyBudget( collocId, day );
            return Ok( DailyBudget );
        }

        [HttpGet( "getMonthlyBudget/{collocId}" )]
        public async Task<IActionResult> GetMothlyBudget( int collocId )
        {
            DateTime month = DateTime.Now;
            IEnumerable<BudgetCatData> DailyBudget = await _budgetGateway.GetMonthlyBudget( collocId, month );
            return Ok( DailyBudget );
        }

        [HttpGet("getBudgetById/{budgetId}", Name="GetBudget")]
        public async Task<IActionResult> GetBudgetById(int budgetId)
        {
            Result<BudgetData> result = await _budgetGateway.FindBudgetById( budgetId );
            return Ok( result );
        }

        [HttpGet( "getBudgetsOfCategory/{categoryId}" )]
        public async Task<IActionResult> GetBudgetsOfCategory ( int categoryId)
        {
            IEnumerable<BudgetData> budgets = await _budgetGateway.GetAllBudgetOfCategory(categoryId);
            return Ok( budgets );
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBudget( [FromBody] BudgetViewModel model)
        {
            
            Result<int> result = await _budgetGateway.CreateBudget( model.CategoryId, model.Date1, model.Date2, model.Amount);
            
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

        [HttpGet("offDates/{categoryId}")]
        public async Task<IActionResult> GetOffDates ( int categoryId)
        {
            Result result = await _budgetGateway.GetOffDates( categoryId );
            return this.CreateResult( result );
        }
    }
}
