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

namespace UnattachedRebarApp
{
    public partial class MainClass : Form
    {
        private readonly Model model = new Model();
        List<Reinforcement> modelObjectsRebarUnattached = new List<Reinforcement>();//pierwsza lista prętów niedopiętych
        List<RebarInfoType> displayListOfRebars = new List<RebarInfoType>();//Lista dla dataGridView
        List<Reinforcement> toSelectionRebars = new List<Reinforcement>();//Lista dla zaznaczenia kilku prętów z listy i pokazaniu w modelu
        List<Guid> GUID = new List<Guid>();//lista Guidów stworzona dla funkcji kliknij i zaznaczy pręt w modelu
        List<Reinforcement> toReportRebarsByOwner = new List<Reinforcement>();//lista stworzona do użycia dla prętów niedopiętych do betonu stworzonych przez jedną osobę
        List<string> owners = new List<string>(); // lista osób, które stworzyły jakikolwiek pręt niedopięty do betonu
        List<Guid> allGUIDs = new List<Guid>();
        public MainClass()
        {
            InitializeComponent();
        }
        public List<Reinforcement> GetModelObjectsRebarUnattached(Model model)
        {
            // metoda tworząca listę prętów nie dołączonych do żadnego cast unitu.
            ModelObjectEnumerator simplerEnumerator = model.GetModelObjectSelector().GetAllObjectsWithType(new Type[] {typeof(Reinforcement)});
            while (simplerEnumerator.MoveNext())
            {
                Reinforcement modelObjectRebar = simplerEnumerator.Current as Reinforcement;
                if (modelObjectRebar != null)
                {
                    string valuePARTGUID = "None";
                    modelObjectRebar.GetReportProperty("PART.GUID", ref valuePARTGUID);
                    if (valuePARTGUID.Length == 36)
                    {
                        continue;
                    }
                    else
                    {
                        modelObjectsRebarUnattached.Add(modelObjectRebar);
                        allGUIDs.Add(modelObjectRebar.Identifier.GUID);
                    }
                }
            }
            return modelObjectsRebarUnattached;
        }
        public List<Reinforcement> GetModelObjectsRebarSelected(Model model, List<Guid> listGUID)
        {
            ModelObjectEnumerator simplerEnumerator = model.GetModelObjectSelector().GetAllObjectsWithType(new Type[] {typeof(Reinforcement)});
            while (simplerEnumerator.MoveNext())
            {
                Reinforcement modelObjectRebar = simplerEnumerator.Current as Reinforcement;
                if (modelObjectRebar != null)
                {
                    if (listGUID.Contains(modelObjectRebar.Identifier.GUID))
                    {
                        toSelectionRebars.Add(modelObjectRebar);
                    }
                }
            }
            return toSelectionRebars;
        }
        public List<Reinforcement> GetReinforcementByOwners(Model model, List<Guid> listGUID, string owner)
        {
            ModelObjectEnumerator simplerEnumerator = model.GetModelObjectSelector().GetAllObjectsWithType(new Type[] {typeof(Reinforcement)});
            while (simplerEnumerator.MoveNext())
            {
                Reinforcement modelObjectRebar = simplerEnumerator.Current as Reinforcement;
                if (modelObjectRebar != null)
                {
                    string rebarOwner = "Empty";
                    modelObjectRebar.GetReportProperty("OWNER", ref rebarOwner);
                    if (listGUID.Contains(modelObjectRebar.Identifier.GUID) && rebarOwner == owner)
                    {
                        toReportRebarsByOwner.Add(modelObjectRebar);
                    }
                }
            }
            return toReportRebarsByOwner;
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
            displayListOfRebars.Clear();
            dataGridView1.DataSource = null;
            {

                List<Reinforcement> modelObjectsRebarUnattached = GetModelObjectsRebarUnattached(model);
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
                string rebarOwner = "Empty";
                reinforcement.GetReportProperty("OWNER", ref rebarOwner);
                RebarInfoType rebarInfo = new RebarInfoType(reinforcement.Name, reinforcement.Identifier.GUID, rebarOwner);
                displayListOfRebars.Add(rebarInfo);
                if (!listBox1.Items.Contains(rebarOwner) && !owners.Contains(rebarOwner))
                {
                    listBox1.Items.Add(rebarOwner);
                    owners.Add(rebarOwner);
                }
            }
            dataGridView1.DataSource = displayListOfRebars;
            listBox1.Sorted = true;


        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AABB rebarBoundingBox = new AABB();
            toSelectionRebars.Clear();
            GUID.Clear();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow rowToSelect = dataGridView1.Rows[e.RowIndex];
                GUID.Add((Guid)rowToSelect.Cells[1].Value);
            }
            {
                List<Reinforcement> toSelectionRebars = GetModelObjectsRebarSelected(model, GUID);
                Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                selector.Select(new System.Collections.ArrayList(toSelectionRebars));
                if (toSelectionRebars != null)
                {
                    Reinforcement rebar = toSelectionRebars[0];
                    Solid rebarSolid = rebar.GetSolid();
                    rebarBoundingBox.MaxPoint = rebarSolid.MaximumPoint;
                    rebarBoundingBox.MinPoint = rebarSolid.MinimumPoint;
                }
                ModelViewEnumerator viewEnum = ViewHandler.GetVisibleViews();
                while (viewEnum.MoveNext())
                {
                    Tekla.Structures.Model.UI.View viewSel = viewEnum.Current;
                    ViewHandler.ZoomToBoundingBox(viewSel, rebarBoundingBox);
                }
            }
        }
        private void Select_in_Model_Click(object sender, EventArgs e)
        {
            GUID.Clear();
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                GUID.Add((Guid)cell.Value);
            }
            toSelectionRebars.Clear();
            {
                List<Reinforcement> toSelectionRebars = GetModelObjectsRebarSelected(model, GUID);
                Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                selector.Select(new System.Collections.ArrayList(toSelectionRebars));
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
                foreach (string ownerInList in listBox1.Items)
                {
                    List<Reinforcement> reinforcements = GetReinforcementByOwners(model, allGUIDs, ownerInList);
                    Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                    selector.Select(new System.Collections.ArrayList(reinforcements));
                    int index = ownerInList.IndexOf("\\");
                    string ownerInListText = ownerInList.Substring(index + 1);
                    System.Threading.Thread.Sleep(1500);
                    Operation.CreateReportFromSelected("PPExample", $"ReportForUser{ownerInListText}.xsr", "", "", "");
                    Operation.DisplayReport($"ReportForUser{ownerInListText}.xsr");
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
                foreach (string ownerInList in listBox1.SelectedItems)
                {
                    List<Reinforcement> reinforcements = GetReinforcementByOwners(model, allGUIDs, ownerInList);
                    Tekla.Structures.Model.UI.ModelObjectSelector selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
                    selector.Select(new System.Collections.ArrayList(reinforcements));
                    int index = ownerInList.IndexOf("\\");
                    string ownerInListText = ownerInList.Substring(index + 1);
                    System.Threading.Thread.Sleep(1500);
                    Operation.CreateReportFromSelected("PPExample", $"ReportForUser{ownerInListText}.xsr", "", "", "");
                    Operation.DisplayReport($"ReportForUser{ownerInListText}.xsr");
                }
            }
        }
    }
}

