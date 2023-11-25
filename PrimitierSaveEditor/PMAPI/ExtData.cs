using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierSaveEditor.PMAPI
{
    [Serializable]
    public class ExtData
    {
        public Dictionary<string, string> ModData { get; set; } = new();
        public List<CubeEIDEntry> EidLocTable { get; set; } = new();

        [Serializable]
        public class CubeEIDEntry
        {
            public string EID { get; set; }
            public int ChunkIndex { get; set; }
            public int GroupIndex { get; set; }
            public int CubeIndex { get; set; }
        }
    }
}
