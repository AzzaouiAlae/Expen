using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Expen.clsUtility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Expen
{
    public class clsWalletData
    {
        async static Task Init()
        {
            if (DB == null)
                DB = new(DatabasePath, flags);

            var table = await DB.CreateTableAsync<clsWallet>();
        }
        public static async Task<List<clsWallet>?> GetAll()
        {
            await Init();
            var wallets = await DB.QueryAsync<clsWallet>($"Select * from [clsWallet]");
            return wallets;
        }
        public static async Task<bool> Add(clsWallet wallet)
        {
            await Init();
            wallet.Date = DateTime.Now;
            int Result = await DB.InsertAsync(wallet);
            return Result > 0;
        }
        public static async Task<bool> Update(clsWallet wallet)
        {
            await Init();
            int Result = await DB.UpdateAsync(wallet);
            return Result > 0;
        }
        public static async Task<bool> Delete(clsWallet wallet)
        {
            await Init();
            int Result = await DB.DeleteAsync(wallet);
            return Result > 0;
        }
        public static async Task<clsWallet?> Find(int id)
        {
            await Init();
            var Wallet = await DB.QueryAsync<clsWallet>($"Select * from [clsWallet] where [ID] = {id}");
            if (Wallet != null && Wallet.Count > 0)
                return Wallet[0];
            return null;
        }
    }
}