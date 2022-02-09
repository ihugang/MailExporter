using System.Collections.Generic;

namespace MailExporter
{
    public class MailboxSetting
    {
        public EnumMailboxType MailboxType { get; set; }

        public string ImapServer { get; set; }

        public int ImapPort { get; set; }

        public List<string> Domains { get; set; }

        public string Tips { get; set; }
    }
}