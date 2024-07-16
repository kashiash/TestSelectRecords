using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.ComponentModel;
using System.Linq;
using TestSelectRecords.Module.BusinessObjects;

namespace TestSelectRecords.Module.Controllers
{
    public class CustomerController : ObjectViewController<DetailView, Customer>
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
            CopyAllUsers(objectSpace, prompt.AvaiableUsers);
            CopyAssignedUsers(objectSpace, prompt.AssignedUsers);
            e.View = e.Application.CreateDetailView(objectSpace, prompt);
        }


        private void CopyAllUsers(IObjectSpace objectSpace, BindingList<UsersDC> users)
        {
            var allUsers = objectSpace.GetObjectsQuery<ApplicationUser>(); //.Where(u => u.IsActive);
            foreach (var user in allUsers)
            {
                var item = objectSpace.CreateObject<UsersDC>();
                item.Oid = user.Oid;
                item.Name = user.UserName;
                users.Add(item);
            }
        }

        private void CopyAssignedUsers(IObjectSpace objectSpace, BindingList<UsersDC> users)
        {

            foreach (var user in ViewCurrentObject.ApplicationUsers)
            {
                var item = objectSpace.CreateObject<UsersDC>();
                item.Oid = user.Oid;
                item.Name = user.UserName;
                users.Add(item);
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


                //delete not selected  users
                //foreach (var user in ViewCurrentObject.ApplicationUsers)
                //{
                //    if (!prompt.AssignedUsers.Any(u => u.Oid == user.Oid))
                //    {
                //        ViewCurrentObject.ApplicationUsers.Remove(user);
                //    }
                //}

                //  ViewCurrentObject.ApplicationUsers.RemoveAll(user => !prompt.AssignedUsers.Any(u => u.Oid == user.Oid));


                // Delete not selected users
                var usersToRemove = ViewCurrentObject.ApplicationUsers
                                    .Where(user => !prompt.AssignedUsers.Any(u => u.Oid == user.Oid))
                                    .ToList();

                foreach (var user in usersToRemove)
                {
                    ViewCurrentObject.ApplicationUsers.Remove(user);
                }

                // assign selected users
                foreach (var item in prompt.AssignedUsers)
                {
                    var user = ObjectSpace.GetObjectByKey<ApplicationUser>(item.Oid);
                    if (user != null)
                    {
                        ViewCurrentObject.ApplicationUsers.Add(user);
                    }
                }
            }
            ObjectSpace.CommitChanges();
            View.Refresh();
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
