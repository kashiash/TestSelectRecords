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
            DostepniPracownicy = new BindingList<UsersDC>();
            PrzypisaniPracownicy = new BindingList<UsersDC>();
        }


        [XafDisplayName("Wybierz zastępców")]
        public BindingList<UsersDC> DostepniPracownicy { get; set; }

        [XafDisplayName("Wybrani zastępcy")]
        public BindingList<UsersDC> PrzypisaniPracownicy { get; set; }
    }

}
