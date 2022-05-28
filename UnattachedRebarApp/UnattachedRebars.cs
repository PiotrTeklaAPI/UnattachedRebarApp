using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using System.Collections;

namespace UnattachedRebarApp
{
    public class UnattachedRebars
    {
        private List<RebarInfo> _rebars;
        IDictionary <string, List<RebarInfo>> rebarsByOwner = new Dictionary<string, List<RebarInfo>>();
        public List<RebarInfo> GetRebars { get { return _rebars; } }
        public List<string> GetOwners()
        {
            //List<string> owners = new List<string>();
            //foreach (RebarInfo rebarinfo in _rebars)
            //{
            //    if (!owners.Contains(rebarinfo.Owner))
            //    {
            //        owners.Add(rebarinfo.Owner);
            //    }
            //}
            return rebarsByOwner.Keys.ToList();

        }
        private void AddNewRebarToCollections(RebarInfo rebarInfo)
        {
            _rebars.Add(rebarInfo);
            if (rebarsByOwner.ContainsKey(rebarInfo.Owner))
            {
                rebarsByOwner[rebarInfo.Owner].Add(rebarInfo);
            }
            else
            {
                rebarsByOwner.Add(rebarInfo.Owner, new List<RebarInfo> { rebarInfo });
            }
        }
        public void LoadUnattachedReabrsFromModel()
        {
            _rebars = new List<RebarInfo>();
            Model model = new Model();
            ModelObjectEnumerator simplerEnumerator = model.GetModelObjectSelector().GetAllObjectsWithType(new Type[] { typeof(Reinforcement) });
            //var progress = new Tekla.Structures.Model.Operations.Operation.ProgressBar();
            //bool displayResult = progress.Display(100, "Checking Reinforcement", "Please wait...", "Cancel", "Label");
            {
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
                            string rebarOwner = "Empty";
                            modelObjectRebar.GetReportProperty("OWNER", ref rebarOwner);
                            
                        }
                    }
                }
            }
        }
    }
}

