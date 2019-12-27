using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Email
{
    public class SMTPServerDTO
    {
        public long Id { get; set; }
        public string SMTP { get; set; }
        public int SMTPport { get; set; }
        public string SenderMailAddress { get; set; }
        public string SenderAccount { get; set; }
        public string SenderPassword { get; set; }
        public bool SslRequired { get; set; }
        public bool AuthRequired { get; set; }
    }
}
