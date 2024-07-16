using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace TestSelectRecords.Module.BusinessObjects
{
    [DomainComponent]
    public class UsersDC : NonPersistentLiteObject
    {

        public string Name { get; set; }
    }
}
