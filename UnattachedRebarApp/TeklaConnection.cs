using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using TSMUI = Tekla.Structures.Model.UI;
using System.Windows.Forms;
using System.Threading;

namespace UnattachedRebarApp
{
    public class TeklaConnection
    {
        private Model _model = new Model();
        public void GenerateReportForObjects(string reportName, string fileName, IEnumerable<Guid> guids)
        {
            TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
            selector.Select(GetTeklaObjectsByGuid(guids));
            Thread.Sleep(1500);
            Operation.CreateReportFromSelected(reportName, fileName, "", "", "");
        }
        private System.Collections.ArrayList GetTeklaObjectsByGuid(IEnumerable<Guid> guids)
        {
            System.Collections.ArrayList teklaObjects = new System.Collections.ArrayList();
            foreach (Guid guid in guids)
            {
                teklaObjects.Add(_model.SelectModelObject(new Tekla.Structures.Identifier(guid)));
            }
            return teklaObjects;
        }
        public System.Collections.ArrayList GetSelectedObjectsFromSheet(DataGridView dataSheet, Model model)
        {
            System.Collections.ArrayList teklaObjectsGuid = new System.Collections.ArrayList();
            foreach (DataGridViewCell cell in dataSheet.SelectedCells)
            {
                teklaObjectsGuid.Add(model.SelectModelObject(new Tekla.Structures.Identifier((Guid)cell.Value)));
            }
            TSMUI.ModelObjectSelector selector = new TSMUI.ModelObjectSelector();
            selector.Select(teklaObjectsGuid);
            return teklaObjectsGuid;
        }
    }


}
