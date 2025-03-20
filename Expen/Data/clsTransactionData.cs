using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Expen.clsUtility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Expen
{
    class clsTransactionData
    {
        async static Task Init()
        {
            if (DB == null)
                DB = new(DatabasePath, flags);

            var table = await DB.CreateTableAsync<clsTransaction>();
        }
        public async static Task<bool> Add(clsTransaction Transaction)
        {
            await Init();
            int Result = await DB.InsertAsync(Transaction);
            return Result > 0;
        }
        public async static Task<bool> Update(clsTransaction Transaction)
        {
            await Init();
            int Result = await DB.UpdateAsync(Transaction);
            return Result > 0;
        }
        public static async Task<bool> Delete(clsTransaction Transaction)
        {
            await Init();
            int Result = await DB.DeleteAsync(Transaction);
            return Result > 0;
        }
        public static async Task<List<clsTransaction>?> GetAll()
        {
            await Init();
            var Transactions = await DB.QueryAsync<clsTransaction>($"Select * from [clsTransaction]");
            return Transactions;
        }
        public static async Task<clsTransaction?> Find(int id)
        {
            await Init();
            var Transactions = await DB.QueryAsync<clsTransaction>($"Select * from [clsTransaction] where [ID] = {id}");
            if (Transactions != null && Transactions.Count >= 0)
                return Transactions[0];
            else
                return null;
        }
        public static async Task<bool> DeleteAllAsync()
        {
            await Init();
            int Result = await DB.DeleteAllAsync<clsTransaction>();
            return Result > 0;
        }
        public static async Task<bool> DropTableAsync()
        {
            await Init();
            int Result = await DB.DropTableAsync<clsTransaction>();
            return Result > 0;
        }
        public static async Task<List<clsTransaction>?> GetLast10Obj()
        {
            await Init();
            var Transactions = await DB.QueryAsync<clsTransaction>($"Select * from [clsTransaction] order by [Date] desc limit 10");
            return Transactions;
        }        
        public static async Task<List<clsTransaction>?> GetAllOrderByDate(DateTime start, DateTime end)
        {
            await Init();
            var Transactions = await DB.QueryAsync<clsTransaction>($"Select * from [clsTransaction] where Date between {start.Ticks} and {end.Ticks}");
            return Transactions;
        }
        public static async Task<List<clsTransaction>?> GetExpensesGroupByCategory(DateTime start, DateTime end)
        {
            await Init();
            var Transactions = await DB.QueryAsync<clsTransaction>($"Select sum(Amount) as Amount, CategoryID from [clsTransaction] where Type = 0 and Date between {start.Ticks} and {end.Ticks} Group by CategoryID");
            return Transactions;
        }
        public static async Task<List<clsTransaction>?> GetIncomeGroupByCategory(DateTime start, DateTime end)
        {
            await Init();
            var Transactions = await DB.QueryAsync<clsTransaction>($"Select sum(Amount) as Amount, CategoryID from [clsTransaction] where Type = 1 and Date between {start.Ticks} and {end.Ticks} Group by CategoryID");
            return Transactions;
        }
    }
}