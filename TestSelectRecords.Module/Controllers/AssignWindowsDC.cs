using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.ComponentModel;
using System.Linq;
using TestSelectRecords.Module.BusinessObjects;

namespace TestSelectRecords.Module.Controllers
{
    [DomainComponent]
    public class AssignWindowsDC : NonPersistentLiteObject
    {
        public AssignWindowsDC()
        {
            AvaiableUsers = new BindingList<UsersDC>();
            AssignedUsers = new BindingList<UsersDC>();
        }


        [XafDisplayName("Wybierz zastępców")]
        public BindingList<UsersDC> AvaiableUsers { get; set; }

        [XafDisplayName("Wybrani zastępcy")]
        public BindingList<UsersDC> AssignedUsers { get; set; }
    }

}
