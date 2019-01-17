using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPOWR.Model
{
    public class FileChunk
    {
        public string fileName { get; set; }
        public string ChunkId { get; set; }
        public double Size { get; set; }
        public bool IsCompleted { get; set; }
        public string OriginalChunkId { get; set; }
        public byte[] ChunkData { get; set; }
    }

    public class SmallChunk
    {
        public string OriginalChunkId { get; set; }
        public string ChunkId { get; set; }
    }
}
