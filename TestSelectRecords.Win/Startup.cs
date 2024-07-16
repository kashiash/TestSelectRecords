using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.Extensions.DependencyInjection;

namespace TestSelectRecords.Win;

public class ApplicationBuilder : IDesignTimeApplicationFactory
{
    public static WinApplication BuildApplication(string connectionString)
    {
        var builder = WinApplication.CreateBuilder();
        // Register custom services for Dependency Injection. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/404430/
        // builder.Services.AddScoped<CustomService>();
        // Register 3rd-party IoC containers (like Autofac, Dryloc, etc.)
        // builder.UseServiceProviderFactory(new DryIocServiceProviderFactory());
        // builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.UseApplication<TestSelectRecordsWindowsFormsApplication>();
        builder.Modules
            .AddAuditTrailXpo()
            .AddCloningXpo()
            .AddConditionalAppearance()
            .AddDashboards(options =>
            {
                options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
                options.DesignerFormStyle = DevExpress.XtraBars.Ribbon.RibbonFormStyle.Ribbon;
            })
            .AddOffice()
            .AddReports(options =>
            {
                options.EnableInplaceReports = true;
                options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
                options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
            })
            .AddScheduler()
            .AddStateMachine(options =>
            {
                options.StateMachineStorageType = typeof(DevExpress.ExpressApp.StateMachine.Xpo.XpoStateMachine);
            })
            .AddValidation(options =>
            {
                options.AllowValidationDetailsAccess = false;
            })
            .AddViewVariants()
            .Add<TestSelectRecords.Module.TestSelectRecordsModule>()
            .Add<TestSelectRecordsWinModule>();
        builder.ObjectSpaceProviders
            .AddSecuredXpo((application, options) =>
            {
                options.ConnectionString = connectionString;
            })

            .AddNonPersistent();


        builder.ObjectSpaceProviders.Events.OnObjectSpaceCreated = context =>
        {
            CompositeObjectSpace compositeObjectSpace = context.ObjectSpace as CompositeObjectSpace;
            if (compositeObjectSpace != null)
            {
                if (!(compositeObjectSpace.Owner is CompositeObjectSpace))
                {
                    var objectSpaceProviderService = context.ServiceProvider.GetRequiredService<IObjectSpaceProviderService>();
                    var objectSpaceCustomizerService = context.ServiceProvider.GetRequiredService<IObjectSpaceCustomizerService>();
                    compositeObjectSpace.PopulateAdditionalObjectSpaces(objectSpaceProviderService, objectSpaceCustomizerService);
                }
            }
        };

        builder.Security
            .UseIntegratedMode(options =>
            {
                options.Lockout.Enabled = true;

                options.RoleType = typeof(PermissionPolicyRole);
                options.UserType = typeof(TestSelectRecords.Module.BusinessObjects.ApplicationUser);
                options.UserLoginInfoType = typeof(TestSelectRecords.Module.BusinessObjects.ApplicationUserLoginInfo);
                options.UseXpoPermissionsCaching();
                options.Events.OnSecurityStrategyCreated += securityStrategy =>
                {
                    // Use the 'PermissionsReloadMode.NoCache' option to load the most recent permissions from the database once
                    // for every Session instance when secured data is accessed through this instance for the first time.
                    // Use the 'PermissionsReloadMode.CacheOnFirstAccess' option to reduce the number of database queries.
                    // In this case, permission requests are loaded and cached when secured data is accessed for the first time
                    // and used until the current user logs out. 
                    // See the following article for more details: https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.PermissionsReloadMode.
                    ((SecurityStrategy)securityStrategy).PermissionsReloadMode = PermissionsReloadMode.NoCache;
                };
            })
            .AddPasswordAuthentication();
        builder.AddBuildStep(application =>
        {
            application.ConnectionString = connectionString;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
            {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        });
        var winApplication = builder.Build();
        return winApplication;
    }

    XafApplication IDesignTimeApplicationFactory.Create()
        => BuildApplication(XafApplication.DesignTimeConnectionString);
}
