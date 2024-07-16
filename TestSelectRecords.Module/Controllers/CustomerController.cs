using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.ComponentModel;
using System.Linq;
using TestSelectRecords.Module.BusinessObjects;

namespace TestSelectRecords.Module.Controllers
{
    public class CustomerController : ObjectViewController<ListView, Customer>
    {
        PopupWindowShowAction assignUsersToCustomerAction;
        public CustomerController() : base()
        {
            // Target required Views (use the TargetXXX properties) and create their Actions.
            assignUsersToCustomerAction = new PopupWindowShowAction(this, $"{GetType().FullName}.{nameof(assignUsersToCustomerAction)}", PredefinedCategory.Edit)
            {
                Caption = "Assign users",
                ImageName = "BO_User"
            };
            assignUsersToCustomerAction.Execute += assignUsersToCustomerAction_Execute;
            assignUsersToCustomerAction.CustomizePopupWindowParams += assignUsersToCustomerAction_CustomizePopupWindowParams;


        }
        private void assignUsersToCustomerAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {

            var objectSpace = (NonPersistentObjectSpace)e.Application.CreateObjectSpace(typeof(AssignWindowsDC));
            var prompt = objectSpace.CreateObject<AssignWindowsDC>();
            CopyUsers(objectSpace, prompt.DostepniPracownicy);
            e.View = e.Application.CreateDetailView(objectSpace, prompt);
        }


        private void CopyUsers(IObjectSpace objectSpace, BindingList<UsersDC> przypisaniPracownicy)
        {
            var users = objectSpace.GetObjectsQuery<ApplicationUser>(); //.Where(u => u.IsActive);
            foreach (var user in users)
            {
                var item = objectSpace.CreateObject<UsersDC>();
                item.Oid = user.Oid;
                item.Name = user.UserName;
                przypisaniPracownicy.Add(item);
            }
        }

        private void assignUsersToCustomerAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var selectedPopupWindowObjects = e.PopupWindowViewSelectedObjects;
            var selectedSourceViewObjects = e.SelectedObjects;
            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112723/).
            ///  var customer = (Customer)View.CurrentObject; // not necessary because we have ViewCurrentObject

            var prompt = e.PopupWindowView.CurrentObject as AssignWindowsDC;
            if (prompt != null)
            {
                foreach (var item in prompt.PrzypisaniPracownicy)
                {
                    var user = ObjectSpace.GetObjectByKey<ApplicationUser>(item.Oid);
                    if (user != null)
                    {
                        ViewCurrentObject.ApplicationUsers.Add(user);
                    }
                }
            }
            ObjectSpace.CommitChanges();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
    }

}
