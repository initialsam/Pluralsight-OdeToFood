using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Email
{
    public class EmailFileDTO
    {
        public string FileName { get; set; }
        public MemoryStream MemoryStream { get; set; }
    }
}
