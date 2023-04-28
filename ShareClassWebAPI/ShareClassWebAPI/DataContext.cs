using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareClassWebAPI.Entities;
using ShareClassWebAPI.Interfaces;
using ShareClassWebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareClassWebAPI
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(this._configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<ClassRoom> DBClassRoom { get; set; }
        public virtual DbSet<ClassRoomsUsers> DBClassRoomsUsers { get; set; }
        public virtual DbSet<HomeTask> DBHomeTask { get; set; }
        public virtual DbSet<HomeTaskFile> DBHomeTaskFile { get; set; }
        public virtual DbSet<Reaction> DBReaction { get; set; }
        public virtual DbSet<ReactionType> DBReactionType { get; set; }
        public virtual DbSet<Solution> DBSolution { get; set; }
        public virtual DbSet<SolutionFile> DBSolutionFile { get; set; }
        public virtual DbSet<User> DBUser { get; set; }

        public int Save()
        {
            return this.SaveChanges();
        }

        private ClassRoomRepository? classRoomRepository;
        public IRepository<ClassRoom> ClassRooms
        {
            get
            {
                if (classRoomRepository == null)
                {
                    classRoomRepository = new ClassRoomRepository(this);
                }
                return classRoomRepository;
            }
        }

        private ClassRoomsUsersRepository? classRoomsUsersRepository;
        public ClassRoomsUsersRepository ClassRoomsUsers
        {
            get
            {
                if (classRoomsUsersRepository == null)
                {
                    classRoomsUsersRepository = new ClassRoomsUsersRepository(this);
                }
                return classRoomsUsersRepository;
            }
        }

        private HomeTaskRepository? homeTaskRepository;
        public IRepository<HomeTask> HomeTasks
        {
            get
            {
                if (homeTaskRepository == null)
                {
                    homeTaskRepository = new HomeTaskRepository(this);
                }
                return homeTaskRepository;
            }
        }

        private HomeTaskFileRepository? HomeTaskFileRepository;
        public IRepository<HomeTaskFile> HomeTaskFiles
        {
            get
            {
                if (HomeTaskFileRepository == null)
                {
                    HomeTaskFileRepository = new HomeTaskFileRepository(this);
                }
                return HomeTaskFileRepository;
            }
        }

        private ReactionRepository? reactionRepository;
        public IRepository<Reaction> Reactions
        {
            get
            {
                if (reactionRepository == null)
                {
                    reactionRepository = new ReactionRepository(this);
                }
                return reactionRepository;
            }
        }

        private ReactionTypeRepository? reactionTypeRepository;
        public IRepository<ReactionType> ReactionTypes
        {
            get
            {
                if (reactionTypeRepository == null)
                {
                    reactionTypeRepository = new ReactionTypeRepository(this);
                }
                return reactionTypeRepository;
            }
        }

        private SolutionRepository? solutionRepository;
        public IRepository<Solution> Solutions
        {
            get
            {
                if (solutionRepository == null)
                {
                    solutionRepository = new SolutionRepository(this);
                }
                return solutionRepository;
            }
        }

        private SolutionFileRepository? SolutionFileRepository;
        public IRepository<SolutionFile> SolutionFiles
        {
            get
            {
                if (SolutionFileRepository == null)
                {
                    SolutionFileRepository = new SolutionFileRepository(this);
                }
                return SolutionFileRepository;
            }
        }

        private UserRepository? userRepository;
        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(this);
                }
                return userRepository;
            }
        }
    }
}