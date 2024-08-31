using Microsoft.EntityFrameworkCore;
using VotingApp.Models;

namespace VotingApp.DbOpeartion
{
    public class VoteContext:DbContext
    {
        public VoteContext(DbContextOptions<VoteContext> options) : base(options) { }

        public DbSet<UserEntity> users { get; set; }
        public DbSet<VotingRoomEntity> room { get; set; }
        public DbSet<PollsEntity> polls { get; set; }
        public DbSet<VoteEntity> votes { get; set; }
        public DbSet<OptionsEntity> options { get; set; }
        public DbSet<RommsEntity> userinroom { get; set; }
    }
}
