using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CareConnectHuddle1.Models
{
    public partial class ITSDataContext : DbContext
    {
        public ITSDataContext()
        {
        }

        public ITSDataContext(DbContextOptions<ITSDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActiveDirectoryRecord> ActiveDirectoryRecords { get; set; }
        public virtual DbSet<CareConnectReportPerson> CareConnectReportPeople { get; set; }
        public virtual DbSet<CareConnectReportTeam> CareConnectReportTeams { get; set; }
        public virtual DbSet<DailySummary> DailySummaries { get; set; }
        public virtual DbSet<DailySummaryBreech> DailySummaryBreeches { get; set; }
        public virtual DbSet<WorkOrderSummary> WorkOrderSummaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MSSQLSERVER; Database=Data; Trusted_Connection=True; Application Name=DailyMetrics; applicationintent=readonly;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ActiveDirectoryRecord>(entity =>
            {
                entity.HasKey(e => e.Sid)
                    .HasName("PK_ActiveDirectoryRecords_1");

                entity.ToTable("ActiveDirectoryRecords", "AD");

                entity.HasIndex(e => e.EmployeeId, "NonClusteredIndex-20180807-EmployeeID");

                entity.HasIndex(e => e.UserAccountControl, "NonClusteredIndex-20190129-140157");

                entity.HasIndex(e => e.CanonicalName, "NonClusteredIndex-Canon");

                entity.HasIndex(e => e.Company, "NonClusteredIndex-Company");

                entity.HasIndex(e => e.DistinguishedName, "NonClusteredIndex-DName");

                entity.HasIndex(e => e.EmployeeNumber, "NonClusteredIndex-EmployeeNumber");

                entity.HasIndex(e => e.IsEnabled, "NonClusteredIndex-Enabled");

                entity.HasIndex(e => e.SamaccountName, "NonClusteredIndex-Sam");

                entity.HasIndex(e => e.System, "NonClusteredIndex-System");

                entity.Property(e => e.Sid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SID");

                entity.Property(e => e.AccountNameHistory)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ActiveDirectory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AdminDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AdminDisplayName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CanonicalName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Company)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Department)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DistinguishedName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.ExtensionAttribute1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute10)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute11)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute12)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute13)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute14)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute15)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute4)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute5)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute6)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute7)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute8)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExtensionAttribute9)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GivenName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Guid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("GUID");

                entity.Property(e => e.Ipphone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IPPhone");

                entity.Property(e => e.IsEnabled)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogon).HasColumnType("datetime");

                entity.Property(e => e.Manager)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MsExchMasterAccountSid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("msExchMasterAccountSid");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NativeGuid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NativeGUID");

                entity.Property(e => e.Office)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OtherName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordLastSet).HasColumnType("datetime");

                entity.Property(e => e.PrimaryGroupId)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PrimaryGroupID");

                entity.Property(e => e.SamaccountName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SAMAccountName");

                entity.Property(e => e.Sn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SN");

                entity.Property(e => e.SurName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.System)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TelephoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserAccountControl)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UserPrincipalName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WhenChanged).HasColumnType("datetime");

                entity.Property(e => e.WhenCreated).HasColumnType("datetime");
            });

            modelBuilder.Entity<CareConnectReportPerson>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TeamId })
                    .HasName("PK_CareConnectReportPeople_8");

                entity.ToTable("CareConnectReportPeople", "BMC");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.CareConnectReportPeople)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CareConnectReportPeople_CareConnectReportTeams");
            });

            modelBuilder.Entity<CareConnectReportTeam>(entity =>
            {
                entity.HasKey(e => e.TeamId)
                    .HasName("PK_CareConnectReportTeams_8");

                entity.ToTable("CareConnectReportTeams", "BMC");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.Property(e => e.LocationHuddleBoard)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LocationIncident)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LocationWorkOrder)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SupportGroup)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DailySummary>(entity =>
            {
                entity.HasKey(e => new { e.ReportDate, e.AssignedOrganization, e.AssignedGroup })
                    .HasName("PK_DailySummary_1");

                entity.ToTable("DailySummary", "BMC");

                entity.Property(e => e.ReportDate).HasColumnType("date");

                entity.Property(e => e.AssignedOrganization)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DailySummaryBreech>(entity =>
            {
                entity.HasKey(e => new { e.ReportDate, e.AssignedOrganization, e.AssignedGroup })
                    .HasName("PK_DailySummaryBreech_1");

                entity.ToTable("DailySummaryBreech", "BMC");

                entity.Property(e => e.ReportDate).HasColumnType("date");

                entity.Property(e => e.AssignedOrganization)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WorkOrderSummary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("WorkOrderSummary", "BMC");

                entity.Property(e => e.AssignedGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedOrganization)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedToNetworkId)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("AssignedToNetworkID");

                entity.Property(e => e.AssignedToRemedyId)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("AssignedToRemedyID");

                entity.Property(e => e.BusinessUnit)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessUnitDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Company)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerNetworkId)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CustomerNetworkID");

                entity.Property(e => e.DateNew).HasColumnType("datetime");

                entity.Property(e => e.DeptDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeptId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("DeptID");

                entity.Property(e => e.EmplId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EmplID");

                entity.Property(e => e.Priority)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RequestManager)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Summary)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SupportGroup)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SupportOrganization)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WorkOrderId)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("WorkOrderID");

                entity.Property(e => e.WorkOrderType)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
