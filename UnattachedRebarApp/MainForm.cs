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
using TSMUI = Tekla.Structures.Model.UI;

namespace UnattachedRebarApp
{
    public partial class MainForm: Form
    {
        private readonly Model model = new Model();
        private UnattachedRebars _unattachedRebars;
        public MainForm()
        {
            InitializeComponent();

            _unattachedRebars = new UnattachedRebars();
        }
        private void Display_Click(object sender, EventArgs e)
        {
            _unattachedRebars.LoadUnattachedReabrsFromModel();
            dataGridView1.DataSource = _unattachedRebars.GetRebars;
            listBoxOwners.DataSource = _unattachedRebars.GetOwners();
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

        

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            TeklaConnection teklaConnection = new TeklaConnection();
            AABB rebarBoundingBox = new AABB();
            {
                ArrayList oneObject = teklaConnection.GetSelectedObjectsFromSheet(dataGridView1, model);
                Reinforcement rebar = (Reinforcement)oneObject[0];
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
        private void Select_in_Model_Click(object sender, EventArgs e)
        {
            TeklaConnection teklaConnection = new TeklaConnection();
            TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
            selector.Select(teklaConnection.GetSelectedObjectsFromSheet(dataGridView1, model));
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void One_report_for_all_owners_Click(object sender, EventArgs e)
        {
            TeklaConnection teklaConnection = new TeklaConnection();
            teklaConnection.GenerateReportForObjects("PPExample", "UnattachedRebarsReportAll.xsr", _unattachedRebars.GetRebars.Select(x => x.Guid));
            Operation.DisplayReport("UnattachedRebarsReportAll.xsr");
        }
        private void Create_separate_for_all_owners_Click(object sender, EventArgs e)
        {
            TeklaConnection teklaConnection = new TeklaConnection();
            if (listBoxOwners.Items == null)
            {
                Operation.DisplayPrompt("No Rebars for report creation.");
            }
            else
            {
                foreach (string owner in listBoxOwners.Items)
                {
                    teklaConnection.GenerateReportForObjects("PPExample", $"Reportfor{owner.Substring(owner.IndexOf("\\") + 1)}.xsr", _unattachedRebars.GetRebarsByOwner(owner).Select(x => x.Guid));
                }
            }
        }
        private void Create_for_selected_Click(object sender, EventArgs e)
        {
            TeklaConnection teklaConnection = new TeklaConnection();
            if (listBoxOwners.SelectedItems.Count == 0)
            {
                Operation.DisplayPrompt("No owner selected from the list.");
            }
            else
            {
                foreach(string owner in listBoxOwners.SelectedItems)
                {
                    teklaConnection.GenerateReportForObjects("PPExample", $"Reportfor{owner.Substring(owner.IndexOf("\\") + 1)}.xsr", _unattachedRebars.GetRebarsByOwner(owner).Select(x => x.Guid));
                    Operation.DisplayReport($"Reportfor{owner.Substring(owner.IndexOf("\\") + 1)}.xsr");
                }
            }
        }
    }
}

