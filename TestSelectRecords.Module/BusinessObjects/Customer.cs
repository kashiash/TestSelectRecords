using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace TestSelectRecords.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Customer : BaseObject
    {
        public Customer(Session session) : base(session)
        { }


        string address;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }

        [Association]
        public XPCollection<ApplicationUser> ApplicationUsers
        {
            get { return GetCollection<ApplicationUser>(nameof(ApplicationUsers)); }
        }




    }
}
