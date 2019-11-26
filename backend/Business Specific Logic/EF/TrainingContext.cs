namespace BusinessSpecificLogic.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Reusable;
    using Reusable.Workflows;

    public partial class TrainingContext : DbContext
    {
        public TrainingContext()
            : base("name=TrainingConn")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            //Database.Log = Console.Write;
        }

        ///Start:Generated:DbSet<<<
		public virtual DbSet<Shift> Shifts { get; set; }
		public virtual DbSet<Certification> Certifications { get; set; }
		public virtual DbSet<JobPosition> JobPositions { get; set; }
		public virtual DbSet<Level1> Level1s { get; set; }
		public virtual DbSet<Level2> Level2s { get; set; }
		public virtual DbSet<Level3> Level3s { get; set; }
		public virtual DbSet<Level4> Level4s { get; set; }
		public virtual DbSet<Level5> Level5s { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<Training> Trainings { get; set; }
		public virtual DbSet<TrainingScore> TrainingScores { get; set; }
		public virtual DbSet<FormatoDC3> FormatoDC3s { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        ///End:Generated:DbSet<<<

        #region From Reusable Modules
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Workflow> Workflows { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
        public virtual DbSet<StepOperation> StepOperations { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            #region Reusable
            modelBuilder.Entity<User>()
                .Property(e => e.Identicon64)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Sorts)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Sort_User_ID);

            #endregion
        }
    }
}
