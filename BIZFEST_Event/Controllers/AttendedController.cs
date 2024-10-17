using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BIZFEST_Event.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendedController : ControllerBase
    {
        private readonly IAttendedUserRepository _repository;
         public AttendedController(IAttendedUserRepository repository)
        {
            _repository=repository;


        }
        [HttpPost]
        public async Task<ReturnStatus<UserAttended>> AttendedUser([FromBody] UserAttended user)
        {
            var result = await _repository.EditUser(user);

            if (result > 0)
            {
                return new ReturnStatus<UserAttended>
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Result = user,
                    Count = 1
                };
            }
            else if(result == -1)
            {
                return new ReturnStatus<UserAttended>
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "User doesn't exists!",
                    Result = user,
                    Count = 0
                };
            }
            else
            {
                return new ReturnStatus<UserAttended>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "You have already taken entry! or QR code has been expired",
                    Result = user,
                    Count = 0
                };
            }
            
        }
    }
}
