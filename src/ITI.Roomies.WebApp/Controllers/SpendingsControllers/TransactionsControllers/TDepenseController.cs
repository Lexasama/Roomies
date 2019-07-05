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
    public class TDepenseController : Controller
    {
        readonly TDepenseGateway _tDepenseGateway;

        public TDepenseController( TDepenseGateway tDepenseGateway )
        {
            _tDepenseGateway = tDepenseGateway;
        }

        [HttpGet( "GetAllTDepense" )]
        public async Task<IActionResult> GetAllTDepense()
        {
            int roomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            IEnumerable<TransacDepenseData> result = await _tDepenseGateway.GetAllTransacDepense( roomieId );
            return Ok( result );
        }

        [HttpGet( "getTDepense/{tDepenseId}", Name = "GetTransacDepense" )]
        public async Task<IActionResult> GetTDepense( int tDepenseId )
        {
            Result<TransacDepenseData> result = await _tDepenseGateway.FindTDepenseById( tDepenseId );
            return this.CreateResult( result );
        }

        [HttpPost( "createTDepense" )]
        public async Task<IActionResult> CreateTDepense( [FromBody] TransacDepenseViewModel model )
        {
            int sRoomieId = int.Parse( HttpContext.User.FindFirst( c => c.Type == ClaimTypes.NameIdentifier ).Value );
            Result<int> result = await _tDepenseGateway.CreateTransacDepense( model.Price, model.Date, sRoomieId, model.RRoomieId );
            return Ok( result );
        }
        [HttpPut( "updateTDepense" )]
        public async Task<IActionResult> UpdateTDepense( [FromBody] TransacDepenseViewModel model )
        {
            Result result = await _tDepenseGateway.UpdateTransacDepense( model.TDepenseId, model.Price, model.Date, model.SRoomieId, model.RRoomieId );
            return this.CreateResult( result );
        }

        [HttpDelete( "deleteTDepense/{tDepenseId}" )]
        public async Task<IActionResult> DeleteTDepense( int tDepenseId )
        {
            Result result = await _tDepenseGateway.DeleteTransacDepense( tDepenseId );
            return this.CreateResult( result );
        }
    }
}
