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
        public DbSet<Registration> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<RegistrationProcess> RegistrationProcesses { get; set; }
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
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<AbsenceRequest> AbsenceRequests { get; set; }
        public DbSet<TransactionImage> TransactionImages { get; set; }
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

            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Registration)
                .WithOne(r => r.Interview)
                .HasForeignKey<Interview>(i => i.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RegistrationProcess>()
                .HasOne(ip => ip.Registration)
                .WithMany(r => r.RegistrationProcesses)
                .HasForeignKey(ip => ip.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Interview>()
                .HasMany(i => i.Accounts)
                .WithMany(a => a.Interviews)
                .UsingEntity<RecruiterInInterview>(
                    l => l.HasOne<Account>(e => e.Account).WithMany(e => e.RecruiterInInterviews).OnDelete(DeleteBehavior.Cascade),
                    r => r.HasOne<Interview>(e => e.Interview).WithMany(e => e.RecruiterInInterviews).OnDelete(DeleteBehavior.Cascade));

            modelBuilder.Entity<CertificateOfCandidate>()
                .HasOne(coc => coc.Registration)
                .WithMany(r => r.CertificateOfCandidates)
                .HasForeignKey(coc => coc.RegistrationId);

            modelBuilder.Entity<Account>()
               .HasOne(a => a.Catechist)
               .WithOne(c => c.Account)
               .HasForeignKey<Catechist>(c => c.AccountId);

            modelBuilder.Entity<TrainingList>()
               .HasMany(tl => tl.Catechists)
               .WithMany(c => c.TrainingLists)
               .UsingEntity<CatechistInTraining>(
                    l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CatechistInTrainings),
                    r => r.HasOne<TrainingList>(e => e.TrainingList).WithMany(e => e.CatechistInTrainings));

            modelBuilder.Entity<Catechist>()
               .HasOne(c => c.ChristianName)
               .WithMany(cn => cn.Catechists)
               .HasForeignKey(c => c.ChristianNameId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Catechist>()
               .HasOne(c => c.Level)
               .WithMany(l => l.Catechists)
               .HasForeignKey(c => c.LevelId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
               .HasOne(c => c.Level)
               .WithMany(l => l.Certificates)
               .HasForeignKey(c => c.LevelId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificate>()
                .HasMany(c => c.Catechists)
                .WithMany(c => c.Certificates)
                .UsingEntity<CertificateOfCatechist>(
                    l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CertificateOfCatechists).OnDelete(DeleteBehavior.Restrict),
                    r => r.HasOne<Certificate>(e => e.Certificate).WithMany(e => e.CertificateOfCatechists).OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Major>()
               .HasMany(m => m.Levels)
               .WithMany(l => l.Majors)
               .UsingEntity<TeachingQualification>(
                   l => l.HasOne<Level>(e => e.Level).WithMany(e => e.TeachingQualifications).OnDelete(DeleteBehavior.Restrict),
                   r => r.HasOne<Major>(e => e.Major).WithMany(e => e.TeachingQualifications).OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Grade>()
               .HasOne(g => g.Major)
               .WithMany(m => m.Grades)
               .HasForeignKey(g => g.MajorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Class>()
               .HasOne(c => c.PastoralYear)
               .WithMany(py => py.Classes)
               .HasForeignKey(c => c.PastoralYearId)
               .OnDelete(DeleteBehavior.Restrict);

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
              .HasForeignKey(s => s.RoomId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Slot>()
              .HasMany(s => s.Catechists)
              .WithMany(c => c.Slots)
              .UsingEntity<CatechistInSlot>(
                  l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CatechistInSlots).OnDelete(DeleteBehavior.ClientSetNull),
                  r => r.HasOne<Slot>(e => e.Slot).WithMany(e => e.CatechistInSlots).OnDelete(DeleteBehavior.ClientSetNull));

            modelBuilder.Entity<Class>()
             .HasOne(c => c.Grade)
             .WithMany(g => g.Classes)
             .HasForeignKey(c => c.GradeId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
             .HasOne(m => m.RoleEvent)
             .WithMany(re => re.Members)
             .HasForeignKey(m => m.RoleEventId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
              .HasMany(e => e.Accounts)
              .WithMany(a => a.Events)
              .UsingEntity<Member>(
                  l => l.HasOne<Account>(e => e.Account).WithMany(e => e.Members).OnDelete(DeleteBehavior.Restrict),
                  r => r.HasOne<Event>(e => e.Event).WithMany(e => e.Members).OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Event>()
               .HasOne(e => e.EventCategory)
               .WithMany(ec => ec.Events)
               .HasForeignKey(p => p.EventCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

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
                     l => l.HasOne<Account>(e => e.Account).WithMany(e => e.MemberOfProcesses).OnDelete(DeleteBehavior.Restrict),
                     r => r.HasOne<Process>(e => e.Process).WithMany(e => e.MemberOfProcesses).OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Grade>()
                 .HasMany(g => g.Catechists)
                 .WithMany(c => c.Grades)
                 .UsingEntity<CatechistInGrade>(
                     l => l.HasOne<Catechist>(e => e.Catechist).WithMany(e => e.CatechistInGrades).OnDelete(DeleteBehavior.Restrict),
                     r => r.HasOne<Grade>(e => e.Grade).WithMany(e => e.CatechistInGrades).OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Post>()
               .HasOne(p => p.PostCategory)
               .WithMany(pc => pc.Posts)
               .HasForeignKey(p => p.PostCategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainingList>()
            .HasOne(tl => tl.NextLevel)
            .WithMany(l => l.NextLevelTrainings)
            .HasForeignKey(tl => tl.NextLevelId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainingList>()
                .HasOne(tl => tl.PreviousLevel)
                .WithMany(l => l.PreviousLevelTrainings)
                .HasForeignKey(tl => tl.PreviousLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainingList>()
            .HasOne(tl => tl.Certificate)
            .WithMany(c => c.TrainingLists)
            .HasForeignKey(tl => tl.CertificateId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AbsenceRequest>(entity =>
            {
                entity.HasOne(ar => ar.Slot)
                    .WithMany(s => s.AbsenceRequests)
                    .HasForeignKey(ar => ar.SlotId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ar => ar.Catechist)
                    .WithMany(c => c.AbsenceRequests)
                    .HasForeignKey(ar => ar.CatechistId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ar => ar.ReplacementCatechist)
                    .WithMany(c => c.ReplacementAbsenceRequests)
                    .HasForeignKey(ar => ar.ReplacementCatechistId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ar => ar.Approver)
                    .WithMany(a => a.AbsenceRequests)
                    .HasForeignKey(ar => ar.ApproverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TransactionImage>()
            .HasOne(ti => ti.BudgetTransaction) 
            .WithMany(bt => bt.TransactionImages)  
            .HasForeignKey(ti => ti.BudgetTransactionId)  
            .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
