using System;

namespace UnattachedRebarApp
{
        public class RebarInfo
        {
            public string Name { get; set; }
            public Guid Guid { get; set; }
            public string Owner { get; set; }
            public RebarInfo(string name, Guid guid, string owner)
            {
                this.Name = name;
                this.Guid = guid;
                this.Owner = owner;
            }
    }
}

