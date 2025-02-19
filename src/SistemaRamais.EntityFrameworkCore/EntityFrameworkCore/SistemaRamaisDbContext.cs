using SistemaRamais.Ramais;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;



namespace SistemaRamais.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class SistemaRamaisDbContext :
    AbpDbContext<SistemaRamaisDbContext>,
    ISaasDbContext,
    IIdentityProDbContext
{
    public DbSet<Ramal> Ramais { get; set; } = null!;
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }



    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public SistemaRamaisDbContext(DbContextOptions<SistemaRamaisDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddictPro();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureGdpr();
        builder.ConfigureBlobStoring();


        if (builder.IsHostDatabase())
        {
            builder.Entity<Ramal>(b =>
            {
                b.ToTable(SistemaRamaisConsts.DbTablePrefix + "Ramais", SistemaRamaisConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Nome).HasColumnName(nameof(Ramal.Nome)).IsRequired().HasMaxLength(RamalConsts.NomeMaxLength);
                b.Property(x => x.Numero).HasColumnName(nameof(Ramal.Numero)).IsRequired().HasMaxLength(RamalConsts.NumeroMaxLength);
                b.Property(x => x.Departamento).HasColumnName(nameof(Ramal.Departamento)).IsRequired().HasMaxLength(RamalConsts.DepartamentoMaxLength);
                b.Property(x => x.Email).HasColumnName(nameof(Ramal.Email)).IsRequired().HasMaxLength(RamalConsts.EmailMaxLength);
                b.Property(x => x.NormalizedName).HasColumnName(nameof(Ramal.NormalizedName)).HasMaxLength(RamalConsts.NormalizedNameMaxLength);
                b.Property(x => x.NormalizedEmail).HasColumnName(nameof(Ramal.NormalizedEmail)).HasMaxLength(RamalConsts.NormalizedEmailMaxLength);
                b.Property(x => x.NormalizedDepartamento).HasColumnName(nameof(Ramal.NormalizedDepartamento)).HasMaxLength(RamalConsts.NormalizedDepartamentoMaxLength);

            });

        }
    }
}