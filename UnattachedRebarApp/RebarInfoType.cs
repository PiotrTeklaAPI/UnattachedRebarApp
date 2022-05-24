using System;

namespace UnattachedRebarApp
{
    public partial class MainClass
    {
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
    }
}

