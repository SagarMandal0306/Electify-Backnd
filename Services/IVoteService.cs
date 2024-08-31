using VotingApp.CustomResult;
using VotingApp.DTO;
using VotingApp.Models;

namespace VotingApp.Services
{
    public interface IVoteService
    {
        Task<int> Register(UserEntityDto user);
        Task<LoginResult> Login(string username,string password);
        Task<UserEntity> GetUserDetails(string username);
        Task<int> CreateRoom(VoteRoomDto vroom);
        Task<List<VotingRoomEntity>> getRooms(int userid);
        Task<int> joinRoom(JoinRoomDto join);
        // Task<int> GetRoomByID(int roomid);
        Task<int> addPoll(PollsDto poll);
        Task<List<PollsEntity>> fetchPolls(int roomid);
        Task<int> addOption(OptionDto op);
        Task<List<OptionsEntity>> fetchOption(int pollid);
        Task<List<UserInfo>> fetchAllUserInRoom(int roomid);
        Task<int> acceptUser(int userid,int roomid,string status);
        Task<int> AddVote(VoteDto vote);
        Task<List<CountsResult>> CountVote(int pollid);
        Task<int> UpdatePoll(PollsUpdateDto poll);
        Task<int> DeletPoll(int pollid);
    }
}
