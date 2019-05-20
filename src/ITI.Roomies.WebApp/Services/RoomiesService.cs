using System;
using System.Threading.Tasks;
using ITI.Roomies.DAL;

namespace ITI.Roomies.WebApp.Services
{
    public class RoomiesService
    {
        readonly RoomiesGateway _roomiesGateway;
        readonly PasswordHasher _passwordHasher;

        public RoomiesService( RoomiesGateway userGateway, PasswordHasher passwordHasher )
        {
            _roomiesGateway = userGateway;
            _passwordHasher = passwordHasher;
        }

        public Task<Result<int>> CreatePasswordUser( string firstName, string lastName, string email, DateTime birthDate, string password, string phone )
        {
            return _roomiesGateway.CreatePasswordUser( firstName, lastName, email, birthDate, _passwordHasher.HashPassword( password ), phone );
        }

        public async Task<RoomiesData> FindUser( string email, string password )
        {
            RoomiesData user = await _roomiesGateway.FindByEmail( email );
            if( user != null && _passwordHasher.VerifyHashedPassword( user.Password, password ) == PasswordVerificationResult.Success )
            {
                return user;
            }

            return null;
        }
    }
}
