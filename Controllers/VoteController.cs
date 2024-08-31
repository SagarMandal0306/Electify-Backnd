using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VotingApp.CustomResult;
using VotingApp.DTO;
using VotingApp.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using VotingApp.Models;


namespace VotingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        protected readonly IVoteService _db;
        protected readonly IConfiguration _configure;
        public VoteController(IVoteService db, IConfiguration configure)
        {
            _db = db;
            _configure = configure;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> signup(UserEntityDto user)
        {
            int result =await _db.Register(user);
            switch (result)
            {
                case 1:
                    return Ok(new { status = "success" });
                case 2:
                    return BadRequest(new { status = "error", user = "true" });
                case 3:
                    return BadRequest(new { status = "error", email = "true" });
                case -1:
                    return BadRequest(new {  status = "internal server error" });


            }
            return BadRequest(new { status = "error" });
        }


        [HttpGet("login")]
        public async Task<IActionResult> login(string username,string password)
        {
            LoginResult result = await _db.Login(username, password);
            switch (result.msgid)
            {
                case 2:
                    return BadRequest(new { status = "error", email = true });
                case 3:
                    return BadRequest(new { status = "error", password = true });
                case -1:
                    return BadRequest(new { status = "error", message = "internal server error" });
            }
            var user = result.user;
            if (user != null)
            {
                var claims = new[]
                {

                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name,user.username),
                    // new Claim(ClaimTypes.Role,user.role)
                    new Claim(ClaimTypes.Role,user.userid)
                    
                   
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configure["JWT:Key"]));

                var token = new JwtSecurityToken(
                   issuer: _configure["JWT:Issuer"],
                   audience: _configure["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)

                    );
                var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new {status="success",token=tokenValue});
               

            }   
            return BadRequest("Internal Server Error");

        }

        [Authorize]
        [HttpGet("getuser")]
        public async Task<IActionResult> getuser()
        {
            var username = User.Identity?.Name;
            if (username != null)
                return Ok(await _db.GetUserDetails(username));
            else
                return Unauthorized("username is not found");
        }

        [Authorize]
        [HttpPost("create/room")]
        public async Task<IActionResult> CreateRoom(VoteRoomDto room)
        {
            int result = await _db.CreateRoom(room);
            if (result == 1)
            {
                return Ok(new { status = "success" });
            }
            else
            {
                return StatusCode(500, new { message = "internal serer error" });
            }
        }

        [Authorize]
        [HttpGet("show/rooms")]
        public async Task<IActionResult> getRooms()
        {
            List<VotingRoomEntity> rooms= await _db.getRooms();
            return Ok(rooms);
        }

        [Authorize]
        [HttpPost("join/room")]
        public async Task<IActionResult> join(JoinRoomDto join)
        {
            int x = await _db.joinRoom(join);
            if (x == 1)
            {
                return Ok(new { status = "success" });
            }else if (x == 2)
            {
                return Ok("user login ");
            }else if (x == 3)
            {
                return Ok("Admin login");
            }
            return StatusCode(500, new { status = "internal server error" });
        }

        [Authorize]
        [HttpPost("add/poll")]
        public async Task<IActionResult> addPolls(PollsDto poll)
        {
            int x = await _db.addPoll(poll);
            if (x == 1)
            {
                return Ok(new { status = "success" });
            }
            else
            {
                return StatusCode(500, new { status = "internal server error" });
            }
        }

        [Authorize]
        [HttpGet("show/polls")]
        public async Task<IActionResult> showpoll(int roomid)
        {
            var polls=  await _db.fetchPolls(roomid);
            if (polls != null)
            {
                return Ok(new { status = "success", polls = polls });
            }
            else
            {
                return StatusCode(500, new { status = "Internal Server Error" });
            }
        }

        [Authorize]
        [HttpPost("add/option")]
        public async Task<IActionResult> option(OptionDto dto)
        {
            int x =await _db.addOption(dto);
            if (x == 1)
            {
                return Ok(new { status = "success" });
            }
            else
            {
                return StatusCode(500, new { status = "Internal Server Error" });
            }
        }

        [Authorize]
        [HttpPost("show/option")]
        public async Task<IActionResult> showOption(int pollid)
        {
            var options = await _db.fetchOption(pollid);
            if (options != null)
            {
                return Ok(options);
            }
            else
            {
                return Ok(null);
            }
        }

        [Authorize]
        [HttpGet("show/rooms/users")]
        public async Task<IActionResult> showUsers(int roomid)
        {
            var users = await _db.fetchAllUserInRoom(roomid);
            if(users != null)
            {
                return Ok(users);
            }
            else
            {
                return Ok(null);
            }
        }


        [Authorize]
        [HttpPut("acceptorreject")]
        public async Task<IActionResult> statusChange(int userid, int roomid, string status)
        {
            int x = await _db.acceptUser(userid, roomid, status);
            switch (x)
            {
                case 1:
                    return Ok(new { Accept = true });
                case 2:
                    return Ok(new { delete = true });
                
            }
            return StatusCode(500, new { status = "Internal Server Error" });
        }

        [Authorize]
        [HttpPost("add/vote")]
        public async Task<IActionResult> addVote(VoteDto vote)
        {
            int x = await _db.AddVote(vote);
            if (x == 1)
            {
                return Ok(new { status = "success" });
            }else if (x == 2)
            {
                return Ok(new { status = "already voted" });
            }else if (x == 3)
            {
                return Ok(new { status = "User is not join the room"});
            }
            else
            {
                return StatusCode(500, new { status = "Internal Server Error" });
            }
        }


        [Authorize]
        [HttpGet("countvote")]
        public async Task<IActionResult> countVote(int pollid)
        {
            var lst = await _db.CountVote(pollid);
            return Ok(lst);
        }

        [Authorize]
        [HttpPut("update/poll")]
        public async Task<IActionResult> UpdatePoll(PollsUpdateDto polls)
        {
            int x = await _db.UpdatePoll(polls);
            if(x==1)
            return Ok(new { status = "success" });
            else
                 return StatusCode(500, new { status = "Internal Server Error" });
        }

        [Authorize]
        [HttpDelete("delete/poll")]
        public async Task<IActionResult> DeletPoll(int pollid)
        {
            int x = await _db.DeletPoll(pollid);
            if (x == 1)
                return Ok(new { status = "success" });
            else
                return StatusCode(500, new { status = "Internal Server Error" });
        }



    }
}
