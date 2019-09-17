using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Romulus.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public uint Credits { get; set; }

        public uint EXP { get; set; }

        public uint LVL
        {
            get
            {
                return (uint)Math.Sqrt(EXP / 25);
            }
        }

        public DateTime LastMessage { get; set; }

        public String Nation { get; set; }
    }
}
