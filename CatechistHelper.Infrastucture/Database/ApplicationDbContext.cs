using CatechistHelper.Domain.Common;
using Microsoft.EntityFrameworkCore;
using CatechistHelper.Domain.Entities;

namespace CatechistHelper.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        #region DbSet
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CatechistHelper.Domain.Entities.Application> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewProcess> InterviewProcesses { get; set; }
        public DbSet<CertificateOfCandidate> CertificateOfCandidates { get; set; }
        public DbSet<Catechist> Catechists { get; set; }
        public DbSet<TrainingList> TrainingLists {  get; set; } 
        public DbSet<ChristianName> ChristianNames { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<PastoralYear> PastoralYears { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<RoleEvent> RoleEvents { get; set; }
        public DbSet<BudgetTransaction> BudgetTransactions { get; set; }
        public DbSet<Process> Processs { get; set; }
        public DbSet<ParticipantInEvent> ParticipantInEvents { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(AppConfig.ConnectionString.DefaultConnection);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Account>().HasIndex(a => a.Email).IsUnique();

            #region Entity Relation
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Account)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AccountId);

            modelBuilder.Entity<Account>()
               .HasOne(a => a.Candidate)
               .WithOne(c => c.Account)
               .HasForeignKey<Candidate>(c => c.AccountId);

            modelBuilder.Entity<CatechistHelper.Domain.Entities.Application>()
                .HasOne(a => a.Candidate)
                .WithMany(c => c.Applications)
                .HasForeignKey(a => a.CandidateId);

            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Application)
                .WithMany(a => a.Interviews)
                .HasForeignKey(i => i.ApplicationId);

            modelBuilder.Entity<InterviewProcess>()
                .HasOne(i => i.Application)
                .WithMany(a => a.InterviewProcesses)
                .HasForeignKey(i => i.ApplicationId);

            modelBuilder.Entity<CatechistHelper.Domain.Entities.Application>()
                .HasMany(a => a.Accounts)
                .WithMany(a => a.Applications)
                .UsingEntity<Recruiter>(
                    l => l.HasOne<Account>(e => e.Account).WithMany(e => e.Recruiters).OnDelete(DeleteBehavior.ClientSetNull),
                    r => r.HasOne<CatechistHelper.Domain.Entities.Application>(e => e.Application).WithMany(e => e.Recruiters).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<CertificateOfCandidate>()
                .HasOne(c => c.Candidate)
                .WithMany(c => c.CertificateOfCandidates)
                .HasForeignKey(c => c.CandidateId);

            modelBuilder.Entity<Account>()
               .HasOne(a => a.Catechist)
               .WithOne(c => c.Account)
               .HasForeignKey<Catechist>(c => c.AccountId);

            modelBuilder.Entity<TrainingList>()
               .HasOne(t => t.Catechist)
               .WithMany(c => c.TrainingLists)
               .HasForeignKey(t => t.CatechistId);

            modelBuilder.Entity<Catechist>()
               .HasOne(c => c.ChristianName)
               .WithMany(c => c.Catechists)
               .HasForeignKey(c => c.ChristianNameId);

            modelBuilder.Entity<Catechist>()
               .HasOne(c => c.Level)
               .WithMany(l => l.Catechists)
               .HasForeignKey(c => c.LevelId);

            modelBuilder.Entity<Certificate>()
               .HasOne(c => c.Level)
               .WithMany(l => l.Certificates)
               .HasForeignKey(c => c.LevelId);

            modelBuilder.Entity<Certificate>()
                .HasMany(c => c.Catechists)
                .WithMany(c => c.Certificates)
                .UsingEntity<CertificateOfCatechist>(
                    l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CertificateOfCatechists).OnDelete(DeleteBehavior.ClientSetNull),
                    r => r.HasOne<Certificate>(e => e.Certificate).WithMany(e => e.CertificateOfCatechists).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<Major>()
               .HasMany(m => m.Levels)
               .WithMany(l => l.Majors)
               .UsingEntity<TeachingQualification>(
                   l => l.HasOne<Level>(e => e.Level).WithMany(e => e.TeachingQualifications).OnDelete(DeleteBehavior.ClientSetNull),
                   r => r.HasOne<Major>(e => e.Major).WithMany(e => e.TeachingQualifications).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<Grade>()
               .HasOne(g => g.Major)
               .WithMany(m => m.Grades)
               .HasForeignKey(g => g.MajorId);

            modelBuilder.Entity<Class>()
               .HasOne(c => c.PastoralYear)
               .WithMany(p => p.Classes)
               .HasForeignKey(c => c.PastoralYearId);

            modelBuilder.Entity<Class>()
               .HasMany(c => c.Catechists)
               .WithMany(c => c.Classes)
               .UsingEntity<CatechistInClass>(
                   l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CatechistInClasses).OnDelete(DeleteBehavior.ClientSetNull),
                   r => r.HasOne<Class>(e => e.Class).WithMany(e => e.CatechistInClasses).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<Slot>()
              .HasOne(s => s.Class)
              .WithMany(c => c.Slots)
              .HasForeignKey(s => s.ClassId);

            modelBuilder.Entity<Slot>()
              .HasOne(s => s.Room)
              .WithMany(r => r.Slots)
              .HasForeignKey(s => s.RoomId);

            modelBuilder.Entity<Slot>()
              .HasMany(s => s.Catechists)
              .WithMany(c => c.Slots)
              .UsingEntity<CatechistInSlot>(
                  l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CatechistInSlots).OnDelete(DeleteBehavior.ClientSetNull),
                  r => r.HasOne<Slot>(e => e.Slot).WithMany(e => e.CatechistInSlots).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<Class>()
             .HasOne(c => c.Grade)
             .WithMany(g => g.Classes)
             .HasForeignKey(c => c.GradeId);

            modelBuilder.Entity<Member>()
             .HasOne(m => m.RoleEvent)
             .WithMany(re => re.Members)
             .HasForeignKey(m => m.RoleEventId);

            modelBuilder.Entity<Event>()
              .HasMany(e => e.Accounts)
              .WithMany(a => a.Events)
              .UsingEntity<Member>(
                  l => l.HasOne<Account>(e => e.Account).WithMany(e => e.Members).OnDelete(DeleteBehavior.ClientSetNull),
                  r => r.HasOne<Event>(e => e.Event).WithMany(e => e.Members).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<BudgetTransaction>()
             .HasOne(bt => bt.Event)
             .WithMany(e => e.BudgetTransactions)
             .HasForeignKey(bt => bt.EventId);

            modelBuilder.Entity<Process>()
               .HasOne(p => p.Event)
               .WithMany(e => e.Processes)
               .HasForeignKey(p => p.EventId);

            modelBuilder.Entity<ParticipantInEvent>()
               .HasOne(pie => pie.Event)
               .WithMany(e => e.ParticipantInEvents)
               .HasForeignKey(pie => pie.EventId);

            modelBuilder.Entity<Process>()
                 .HasMany(p => p.Accounts)
                 .WithMany(a => a.Processes)
                 .UsingEntity<MemberOfProcess>(
                     l => l.HasOne<Account>(e => e.Account).WithMany(e => e.MemberOfProcesses).OnDelete(DeleteBehavior.ClientSetNull),
                     r => r.HasOne<Process>(e => e.Process).WithMany(e => e.MemberOfProcesses).OnDelete(DeleteBehavior.ClientSetNull));
            #endregion
        }
    }
}
