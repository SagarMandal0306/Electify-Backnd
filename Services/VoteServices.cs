using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using VotingApp.CustomResult;
using VotingApp.DbOpeartion;
using VotingApp.DTO;
using VotingApp.Models;

namespace VotingApp.Services
{
    public class VoteServices : IVoteService
    {
        protected readonly VoteContext _context;
        protected readonly IPasswordHasher<UserEntityDto> _hash;
        public VoteServices(VoteContext context)
        {
            _context = context;
            _hash = new PasswordHasher<UserEntityDto>();
        }

        public async Task<int> Register(UserEntityDto user)
        {
            try
            {
                bool findByUser=await _context.users.AnyAsync(x=>x.username == user.username);
                if (findByUser)
                {
                    return await Task.FromResult(2);
                }
                bool findbyemail=await _context.users.AnyAsync(x => x.useremail == user.useremail);
                if (findbyemail)
                {
                    return await Task.FromResult(3);
                }
                string hashpass = _hash.HashPassword(user, user.password);
                UserEntity obj = new UserEntity
                {
                    username = user.username,
                    useremail=user.useremail,
                    password = hashpass,
                    role = "user"
                };

                await _context.users.AddAsync(obj);
                int x = _context.SaveChanges();
                return await Task.FromResult(x);
            }
            catch
            {
                return -1;
            }
        }

        public async Task<LoginResult> Login(string username, string password)
        {
            try
            {
                UserEntity user = await _context.users.FirstOrDefaultAsync(x => x.username == username);
                if (user == null)
                {
                    return new LoginResult { msgid = 2, user = null };
                }
                string hashpass = user.password;
                var isPassword = _hash.VerifyHashedPassword(null, hashpass, password);
                if (isPassword == PasswordVerificationResult.Success)
                {
                    return new LoginResult { msgid = 1, user = user };
                }
                else
                {
                    return new LoginResult { msgid = 3, user = null };
                }
            }
            catch
            {
                return new LoginResult { msgid = -1, user = null };
            }
            
        }

        public async Task<UserEntity> GetUserDetails(string username)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.username == username);
                if (user != null)
                {
                    return await Task.FromResult(user);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> CreateRoom(VoteRoomDto vroom)
        {
            try
            {
                int randomNumber;
                do
                {
                    Random random = new Random();
                    randomNumber = random.Next(10000000, 100000000);
                } while (await _context.room.AnyAsync(r => r.roomid == randomNumber));
               
                VotingRoomEntity room = new VotingRoomEntity
                {
                    roomid = randomNumber,
                    userid = vroom.userid,
                    title = vroom.title,
                    desc = vroom.desc,
                    createdat = DateTime.Now


                };
               
               
                await _context.room.AddAsync(room);
                int x = await _context.SaveChangesAsync();
                return await Task.FromResult(x);
            }
            catch
            {
                return -1;
            }
        }

        public async Task<List<VotingRoomEntity>> getRooms(int userid)
        {
            try
            {
                var context =  from uroom in _context.userinroom
                              join room in _context.room
                              on uroom.roomid equals room.roomid
                              where uroom.userid == userid  
                              select new VotingRoomEntity{
                                id=room.id,
                                title=room.title,
                                desc=room.desc,
                                roomid=room.roomid,
                                userid=room.userid,
                              };
                              return await Task.FromResult(context);
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> joinRoom(JoinRoomDto join)
        {
            try
            {
                UserEntity user = await _context.users.FirstOrDefaultAsync(x => x.username == join.username);
                if (user != null)
                {
                    var findbyuser = await _context.userinroom.FirstOrDefaultAsync(x => x.userid == user.userid );
                    var admincheck = await _context.room.FirstOrDefaultAsync(x => x.roomid == join.roomid );
                    if (findbyuser?.roomid==join.roomid )
                    {
                        return 2;
                    }
                    if (admincheck?.userid == user.userid)
                    {
                        return 3;
                    }
                   
                    RommsEntity entity = new RommsEntity
                    {
                        roomid = join.roomid,
                        userid = user.userid,
                        role = user.role,
                        status = "pending"

                    };
                    await _context.userinroom.AddAsync(entity);
                    return await _context.SaveChangesAsync();
                }
                return 0;
                
            }
            catch
            {
                return -1;
            }
        }

        // public async Task<int> GetRoomByID(int roomid){
        //     try{

        //     }catch{
        //         return
        //     }
        // }

        public async Task<int> addPoll(PollsDto poll)
        {
            try
            {
                int min = poll.durationInMinutes;
                int hr = poll.durationInHours;
                string endAt = DateTime.UtcNow.AddHours(hr).AddMinutes(min).ToLocalTime().ToString();

                PollsEntity obj = new PollsEntity
                {
                    title = poll.title,
                    desc = poll.desc,
                    roomid = poll.roomid,
                    createat = DateTime.UtcNow.ToLocalTime().ToString(),
                    endat = endAt
                };

                await _context.polls.AddAsync(obj);
                return await _context.SaveChangesAsync();
            }catch
            {
                return -1;
            }
           

        }

        public async Task<List<PollsEntity>> fetchPolls(int roomid)
        {
            try
            {
                var polls = await _context.polls.FirstOrDefaultAsync(x => x.roomid == roomid);
                if (polls != null)
                {
                    return await _context.polls.ToListAsync();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
           
        }

        public async Task<int> addOption(OptionDto op)
        {
            try
            {
                var poll = await _context.polls.OrderByDescending(x => x.pollid).FirstOrDefaultAsync(x=>x.roomid==op.roomid);
                OptionsEntity option = new OptionsEntity
                {
                    pollid = poll.pollid,
                    option = op.option
                };
                await _context.options.AddAsync(option);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
        }

        public async Task<List<OptionsEntity>> fetchOption(int pollid)
        {
            try
            {
                return await _context.options.Where(x=>x.pollid==pollid).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<UserInfo>> fetchAllUserInRoom(int roomid)
        {
            try
            {
                var context = from room in _context.userinroom
                              join user in _context.users
                              on room.userid equals user.userid
                              where room.roomid == roomid 
                              select new UserInfo
                              {
                                  userid=user.userid,
                                  username=user.username,
                                  status=room.status
                              };
                return await context.ToListAsync();

            }
            catch
            {
                return null;
            }
        }

        public async Task<int> acceptUser(int userid, int roomid,string status)
        {
            try
            {
                var user = await _context.userinroom.FirstOrDefaultAsync(x => x.userid == userid && x.roomid == roomid);
                if (user != null)
                {
                    if (status == "accept")
                    {
                        user.status = "accept";
                         await _context.SaveChangesAsync();
                        return 1;
                    }
                    else if (status == "delete")
                    {
                        _context.userinroom.Remove(user);
                         await _context.SaveChangesAsync();
                        return 2;
                    }
                }
                return -1;
               
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> AddVote(VoteDto vote)
        {
            try
            {
                bool voteedornot = await _context.votes.AnyAsync(x => x.userid == vote.userid && x.pollid == vote.pollid);
                bool isuserinroom = await _context.userinroom.AnyAsync(x => x.userid == vote.userid);
                if (voteedornot)
                {
                    return 2;
                }
                if (!isuserinroom)
                {
                    return 3;
                }
                VoteEntity obj = new VoteEntity
                {
                    userid = vote.userid,
                    pollid = vote.pollid,
                    optionid = vote.optionid,
                    votedat = DateTime.UtcNow.ToLocalTime()
                };
                await _context.votes.AddAsync(obj);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
          
        }

        public async Task<List<CountsResult>> CountVote( int pollid)
        {
            try
            {
                List<CountsResult> lst = new List<CountsResult>();
                int countVote = await _context.votes.CountAsync(x => x.pollid == pollid);
                lst.Add( new CountsResult { name="poll",id=pollid,total=countVote});

                var obj = await _context.options.Where(x => x.pollid == pollid).ToListAsync();
                foreach(var op in obj)
                {
                    int countVotePerOption = await _context.votes.CountAsync(x => x.pollid == pollid && x.optionid == op.optionid);
                    lst.Add(new CountsResult { name = "option", id = op.optionid, total = countVotePerOption });
                }

                return await Task.FromResult(lst);
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> UpdatePoll(PollsUpdateDto polls)
        {
            try
            {
                var poll = await _context.polls.FindAsync(polls.pollid);
                var creatadat = DateTime.Parse(poll?.createat);
                var endat = creatadat.AddHours(polls.durationInHours).AddMinutes(polls.durationInMinutes);
                if (poll != null)
                {
                    poll.title = polls.title;
                    poll.desc=poll.desc;
                    poll.endat = endat.ToString();
                }
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> DeletPoll(int pollid)
        {
            try
            {
                var poll = await _context.polls.FindAsync(pollid);
                _context?.Remove(poll);
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
            

        }
    }
}
