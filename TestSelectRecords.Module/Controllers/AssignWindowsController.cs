using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using System;
using System.Linq;
using TestSelectRecords.Module.BusinessObjects;

namespace TestSelectRecords.Module.Controllers
{
    public class AssignWindowsController : ObjectViewController<DetailView, AssignWindowsDC>
    {
        SimpleAction copySelectedAction;
        SimpleAction removeSelectedAction;
        public AssignWindowsController() : base()
        {
            // Target required Views (use the TargetXXX properties) and create their Actions.
            copySelectedAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(copySelectedAction)}", "CustomActionContainer")
            {
                Caption = ">>"
            };
            copySelectedAction.Execute += copySelectedAction_Execute;


            // Target required Views (use the TargetXXX properties) and create their Actions.
            removeSelectedAction = new SimpleAction(this, $"{GetType().FullName}.{nameof(removeSelectedAction)}", "CustomActionContainer")
            {
                Caption = "<<"
            };
            removeSelectedAction.Execute += deleteSelectedAction_Execute;

        }

        private void deleteSelectedAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var currentObject = (AssignWindowsDC)View.CurrentObject;
            ListPropertyEditor listPropertyEditor = View.FindItem(nameof(AssignWindowsDC.AssignedUsers)) as ListPropertyEditor;
            if (listPropertyEditor != null)
            {
                ListView nestedListView = listPropertyEditor.ListView;
                if (nestedListView != null)
                {
                    // Get selected records
                    var selectedRecords = nestedListView.SelectedObjects.Cast<UsersDC>().ToList();
                    foreach (var record in selectedRecords)
                    {
                        currentObject.AssignedUsers.Remove(record);
                    }
                }
            }
        }

        private void copySelectedAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112737/).
            // Access the nested ListView
            var currentObject = (AssignWindowsDC)View.CurrentObject;
            ListPropertyEditor listPropertyEditor = View.FindItem(nameof(AssignWindowsDC.AvaiableUsers)) as ListPropertyEditor;
            if (listPropertyEditor != null)
            {
                ListView nestedListView = listPropertyEditor.ListView;
                if (nestedListView != null)
                {
                    // Get selected records
                    var selectedRecords = nestedListView.SelectedObjects.Cast<UsersDC>().ToList();
                    foreach (var record in selectedRecords)
                    {

                        currentObject.AssignedUsers.Add(record);
                    }
                }
            }
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
