using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Romulus.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;

        private static string accountsFile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (DataStorage.SaveExists(accountsFile))
            {
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        // save accounts to json file
        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
        }

        // gets account of a user
        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        // gets or creates account of a user using json file
        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.ID == id
                         select a;

            var account = result.FirstOrDefault();
            if (account == null)
                account = CreateUserAccount(id);

            return account;
        }

        // create an account for a user in json file
        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount()
            {
                ID = id,
                Credits = 20,
                EXP = 0,
                LastMessage = DateTime.Now
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
