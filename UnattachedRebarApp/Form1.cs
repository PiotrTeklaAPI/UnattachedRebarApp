using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using TSG = Tekla.Structures.Geometry3d;
using System.Collections;
using Tekla.Structures.Analysis;
using System.Threading;

namespace UnattachedRebarApp___DataGridView
{
    public partial class Form1 : Form
    {
        private readonly Model model = new Model();
        List<Reinforcement> modelObjectsRebarUnattached = new List<Reinforcement>();//pierwsza lista prętów niedopiętych
        List<RebarInfoType> DisplayListOfRebars = new List<RebarInfoType>();//Lista dla dataGridView
        List<Reinforcement> ToSelectionRebars = new List<Reinforcement>();//Lista dla zaznaczenia kilku prętów z listy i pokazaniu w modelu
        List<Guid> GUID = new List<Guid>();//lista Guidów stworzona dla funkcji kliknij i zaznaczy pręt w modelu
        List<Reinforcement> ToReportRebarsByOwner = new List<Reinforcement>();//lista stworzona do użycia dla prętów niedopiętych do betonu stworzonych przez jedną osobę
        List<string> Owners = new List<string>(); // lista osób, które stworzyły jakikolwiek pręt niedopięty do betonu
        List<Guid> AllGUIDs = new List<Guid>();
        public Form1()
        {
            InitializeComponent();
        }
        public class RebarInfoType
        {
            public string NameofRebar { get; set; }
            public Guid GUIDOfRebar { get; set; }
            public string OwnerOfRebar { get; set; }
            public RebarInfoType(string name, Guid GUID, string owner)
            {
                this.NameofRebar = name;
                this.GUIDOfRebar = GUID;
                this.OwnerOfRebar = owner;
            }
        }
        public List<Reinforcement> getModelObjectsRebarUnattached(Model model)
        {
            // metoda tworząca listę prętów nie dołączonych do żadnego cast unitu.
            System.Type[] TypeReinforcement = new System.Type[1];
            TypeReinforcement.SetValue(typeof(Reinforcement), 0);
            ModelObjectEnumerator enumerator = model.GetModelObjectSelector().GetAllObjectsWithType(TypeReinforcement);
            while (enumerator.MoveNext())
            {
                Reinforcement modelObjectRebar = enumerator.Current as Reinforcement;
                if (modelObjectRebar != null)
                {
                    string ValuePARTGUID = "None";
                    modelObjectRebar.GetReportProperty("PART.GUID", ref ValuePARTGUID);
                    if (ValuePARTGUID.Length == 36)
                    {
                        continue;
                    }
                    else
                    {
                        modelObjectsRebarUnattached.Add(modelObjectRebar);
                        AllGUIDs.Add(modelObjectRebar.Identifier.GUID);
                    }
                }
            }
                return modelObjectsRebarUnattached;
        }
        public List<Reinforcement> getModelObjectsRebarSelected(Model model, List <Guid> GUIDList)
        {
            System.Type[] TypeReinforcement = new System.Type[1];
            TypeReinforcement.SetValue(typeof(Reinforcement), 0);
            ModelObjectEnumerator enumerator = model.GetModelObjectSelector().GetAllObjectsWithType(TypeReinforcement);
            while (enumerator.MoveNext())
            {
                Reinforcement modelObjectRebar = enumerator.Current as Reinforcement;
                if (modelObjectRebar != null)
                {
                    if(GUIDList.Contains(modelObjectRebar.Identifier.GUID)) 
                    { 
                        ToSelectionRebars.Add(modelObjectRebar);
                    }
                }
            }
            return ToSelectionRebars;
        }
        public List<Reinforcement> getReinforcementByOwners(Model model, List <Guid> GUIDList, string Owner)
        {
            System.Type[] TypeReinforcement = new System.Type[1];
            TypeReinforcement.SetValue(typeof(Reinforcement), 0);
            ModelObjectEnumerator enumerator = model.GetModelObjectSelector().GetAllObjectsWithType(TypeReinforcement);
            while (enumerator.MoveNext())
            {
                Reinforcement modelObjectRebar = enumerator.Current as Reinforcement;
                if (modelObjectRebar != null)
                {
                    string RebarOwner = "Empty";
                    modelObjectRebar.GetReportProperty("OWNER", ref RebarOwner);
                    if (GUIDList.Contains(modelObjectRebar.Identifier.GUID) && RebarOwner == Owner)
                    {
                        ToReportRebarsByOwner.Add(modelObjectRebar);
                    }
                }
            }
            return ToReportRebarsByOwner;
        }
            public void Form1_Load(object sender, EventArgs e)
        {
            if (!model.GetConnectionStatus())
            {
                Operation.DisplayPrompt("Tekla Structures not connected!");
                return;
            }
            else
            {
                Operation.DisplayPrompt((string.Format("Connected to Tekla!")));
                return;
            }
        }

        private void Display_Click(object sender, EventArgs e)
        {
            modelObjectsRebarUnattached.Clear();
            DisplayListOfRebars.Clear();
            dataGridView1.DataSource = null;
            {

                List<Reinforcement> modelObjectsRebarUnattached = getModelObjectsRebarUnattached(model);
                if (modelObjectsRebarUnattached.Count <= 0)
                {
                    Operation.DisplayPrompt("No unattached rebars detected!");
                }
                else
                {
                    Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                    selector.Select(new System.Collections.ArrayList(modelObjectsRebarUnattached));
                }
            }
            foreach (Reinforcement reinforcement in modelObjectsRebarUnattached)
            {
                string RebarOwner = "Empty";
                reinforcement.GetReportProperty("OWNER", ref RebarOwner);
                RebarInfoType rebarInfo = new RebarInfoType(reinforcement.Name, reinforcement.Identifier.GUID, RebarOwner);
                DisplayListOfRebars.Add(rebarInfo);
                if (!listBox1.Items.Contains(RebarOwner) && !Owners.Contains(RebarOwner))
                {
                    listBox1.Items.Add(RebarOwner);
                    Owners.Add(RebarOwner);
                }  
            }
            dataGridView1.DataSource = DisplayListOfRebars;
            listBox1.Sorted = true;
            
            
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AABB PartBoundingBox = new AABB();
            ToSelectionRebars.Clear();
            GUID.Clear();
            if (e.RowIndex >= 0)
            {
                    DataGridViewRow RowToSelect = dataGridView1.Rows[e.RowIndex];
                    GUID.Add((Guid)RowToSelect.Cells[1].Value);          
            }
            {
                List<Reinforcement> ToSelectionRebars = getModelObjectsRebarSelected(model, GUID);
                Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                selector.Select(new System.Collections.ArrayList(ToSelectionRebars));
                if (ToSelectionRebars != null)
                {
                    Reinforcement Rebar = ToSelectionRebars[0];
                    Solid rebarSolid = Rebar.GetSolid();
                    PartBoundingBox.MaxPoint = rebarSolid.MaximumPoint;
                    PartBoundingBox.MinPoint = rebarSolid.MinimumPoint;
                }
                ModelViewEnumerator ViewEnum = ViewHandler.GetVisibleViews();
                while (ViewEnum.MoveNext())
                {
                    Tekla.Structures.Model.UI.View ViewSel = ViewEnum.Current;
                    ViewHandler.ZoomToBoundingBox(ViewSel, PartBoundingBox);
                }
            }
        }
        private void Select_in_Model_Click(object sender, EventArgs e)
        {
            GUID.Clear();
            foreach (DataGridViewCell Cell in dataGridView1.SelectedCells)
            {
                GUID.Add((Guid)Cell.Value);
            }
            ToSelectionRebars.Clear();
            {
                List<Reinforcement> ToSelectionRebars = getModelObjectsRebarSelected(model, GUID);
                Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                selector.Select(new System.Collections.ArrayList(ToSelectionRebars));
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void One_report_for_all_owners_Click(object sender, EventArgs e)
        {
            Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            selector.Select(new System.Collections.ArrayList(modelObjectsRebarUnattached));
            System.Threading.Thread.Sleep(1500);
            Operation.CreateReportFromSelected("PPExample", "UnattachedRebarsReportAll.xsr", "", "", "");
            Operation.DisplayReport("UnattachedRebarsReportAll.xsr");
        }
        private void Create_separate_for_all_owners_Click(object sender, EventArgs e)//działa, choć trzeba sprawdzić na modelu sharing
        {
            if (listBox1.Items == null)
            {
                Operation.DisplayPrompt("No Rebars for report creation.");
            }
            else
            {
                foreach (string OwnerInList in listBox1.Items)
                {
                    List<Reinforcement> reinforcements = getReinforcementByOwners(model, AllGUIDs, OwnerInList);
                    Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                    selector.Select(new System.Collections.ArrayList(reinforcements));
                    int index = OwnerInList.IndexOf("\\");
                    string OwnerInListText = OwnerInList.Substring(index + 1);
                    System.Threading.Thread.Sleep(1500);
                    Operation.CreateReportFromSelected("PPExample", $"ReportForUser{OwnerInListText}.xsr", "", "", "");
                    Operation.DisplayReport($"ReportForUser{OwnerInListText}.xsr");
                }
            }
        }
        private void Create_for_selected_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                Operation.DisplayPrompt("No owner selected from the list.");
            }
            else
            {
                foreach (string OwnerInList in listBox1.SelectedItems)
                {
                    List<Reinforcement> reinforcements = getReinforcementByOwners(model, AllGUIDs, OwnerInList);
                    Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                    selector.Select(new System.Collections.ArrayList(reinforcements));
                    int index = OwnerInList.IndexOf("\\");
                    string OwnerInListText = OwnerInList.Substring(index + 1);
                    System.Threading.Thread.Sleep(1500);
                    Operation.CreateReportFromSelected("PPExample", $"ReportForUser{OwnerInListText}.xsr", "", "", "");
                    Operation.DisplayReport($"ReportForUser{OwnerInListText}.xsr");
                }
            }
        }
    }
}

