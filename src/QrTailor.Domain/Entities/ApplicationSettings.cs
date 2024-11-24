using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Domain.Entities
{
    public class ApplicationSettings
    {
        public static string DbConnString { get; set; }
        public static string AppName { get; set; }
        public static string MailHost { get; set; }
        public static int MailPort { get; set; }
        public static string MailFrom { get; set; }
        public static string MailPassword { get; set; }
        public static bool MailEnableSsl { get; set; }

    }
}
