using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System.ComponentModel;

namespace TestSelectRecords.Module.BusinessObjects;

[MapInheritance(MapInheritanceType.ParentTable)]
[DefaultProperty(nameof(UserName))]
[DefaultClassOptions]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo, ISecurityUserLockout
{
    private int accessFailedCount;
    private DateTime lockoutEnd;

    public ApplicationUser(Session session) : base(session) { }

    [Browsable(false)]
    public int AccessFailedCount
    {
        get { return accessFailedCount; }
        set { SetPropertyValue(nameof(AccessFailedCount), ref accessFailedCount, value); }
    }

    [Browsable(false)]
    public DateTime LockoutEnd
    {
        get { return lockoutEnd; }
        set { SetPropertyValue(nameof(LockoutEnd), ref lockoutEnd, value); }
    }

    [Browsable(false)]
    [Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo
    {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }


    [Association]
    public XPCollection<Customer> Customers
    {
        get { return GetCollection<Customer>(nameof(Customers)); }
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
    {
        ApplicationUserLoginInfo result = new ApplicationUserLoginInfo(Session);
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }
}
