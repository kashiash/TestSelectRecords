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
            DostepniPracownicy = new BindingList<ApplicationUser>();
            PrzypisaniPracownicy = new BindingList<ApplicationUser>();
        }


        [XafDisplayName("Wybierz zastępców")]
        public BindingList<ApplicationUser> DostepniPracownicy { get; set; }

        [XafDisplayName("Wybrani zastępcy")]
        public BindingList<ApplicationUser> PrzypisaniPracownicy { get; set; }
    }

}
