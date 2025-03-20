using Microsoft.VisualBasic;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expen
{
    public class clsTransaction
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public byte Type { get; set; } //0 = Expense | 1 = Income | 2 = Transfer
        public int CategoryID { get; set; }
        public int WalletID { get; set; }
        public int ToWalletID { get; set; }

        clsCategories? _Category;

        [Ignore]
        public clsCategories? Category
        {
            get
            {
                return _Category;
            }
        }

        clsWallet? _wallet;
        [Ignore]
        public clsWallet? wallet
        {
            get
            {
                return _wallet;
            }
        }

        clsWallet? _ToWallet;
        [Ignore]
        public clsWallet? ToWallet
        {
            get
            {
                return _ToWallet;
            }
        }
        async Task<bool> UndoTransaction()
        {
            clsTransaction? t = await Find(ID);
            if (t == null) return false;
            if (!(await t.LoadWallet())) return false;
            if (!(await t.LoadCategory())) return false;

            if (t.Type == 2 && t.ToWallet == null) return false;
            if (t.wallet == null) return false;
            if (t.Type == 1)
            {
                if (!(await t.wallet.Withdrawal(t.Amount, true))) return false;
            }
            else if (t.Type == 0)
            {
                if (!(await t.wallet.Deposit(t.Amount))) return false;
            }
            else if (t.Type == 2)
            {
                if (!(await t.ToWallet.Withdrawal(t.Amount, true))) return false;
                if (!(await t.wallet.Deposit(t.Amount))) return false;
            }
            return true;
        }
        async Task<bool> PerformTransaction()
        {
            _wallet = await clsWallet.Find(WalletID);
            if (Type == 2)            
                _ToWallet = await clsWallet.Find(ToWalletID);            

            if(Type == 2 && ToWallet == null) return false;
            if (wallet == null) return false;
            if (Type == 1)
            {
                if (!(await wallet.Deposit(Amount))) return false;
            }
            else if (Type == 0)
            {
                if (!(await wallet.Withdrawal(Amount))) return false;
            }
            else if (Type == 2)
            {
                if (ToWallet == null) return false;
                if (!(await wallet.Withdrawal(Amount))) return false;
                if (!(await ToWallet.Deposit(Amount))) return false;
            }
            return true;
        }
        public async Task<bool> Save()
        {
            Log = "";
            bool Result = false;
            if (!(await LoadWallet())) return false;           
            if (!(await LoadCategory())) return false;

            if (ID == -1)
            {
                if (!await PerformTransaction()) return false;

                Result = await clsTransactionData.Add(this);
            }
            else
            {
                if(!await UndoTransaction() ) return false;

                if (!await PerformTransaction()) return false;

                Result = await clsTransactionData.Update(this);
            }
            if(Result)
                Saved?.Invoke(this);
            return Result;
        }
        public async static Task<bool> Delete(int id)
        {
            return await clsTransactionData.Delete(new clsTransaction() { ID = id });
        }
        public static async Task<List<clsTransaction>?> GetAll()
        {
            return await clsTransactionData.GetAll();
        }
        public async Task<bool> LoadCategory()
        {
            if(Type == 2)
            {
                if (Category == null)
                    _Category = await clsCategories.Find("Transfer");

                if(Category != null)
                    CategoryID = Category.ID;

                if (Category == null) { Log = "failed to Load Category"; return false; }
                else { Log = ""; return true; }
            }
            else
            {
                if (Category == null)
                    _Category = await clsCategories.Find(CategoryID);

                if (Category != null && CategoryID != Category.ID) _Category = await clsCategories.Find(CategoryID);

                if (Category == null) { Log = "failed to Load Category"; return false; }
                else { Log = ""; return true; }
            }            
        }
        public async Task<bool> LoadWallet()
        {
            if (_wallet == null)
                _wallet = await clsWallet.Find(WalletID);

            if(Type == 2)
            {
                if(ToWalletID != 0 && _ToWallet == null)
                    _ToWallet = await clsWallet.Find(ToWalletID);
            }

            if(_wallet != null && WalletID != _wallet.ID) _wallet = await clsWallet.Find(WalletID);

            if (Type == 2)
            {
                if (_ToWallet != null && ToWalletID != _ToWallet.ID)
                    _ToWallet = await clsWallet.Find(ToWalletID);
            }

            if (_wallet == null || (Type == 2 && _ToWallet == null) ) { Log = "failed to Load Wallet"; return false; }
            else { Log = ""; return true; }
        }
        public static async Task<clsTransaction?> Find(int id)
        {
            return await clsTransactionData.Find(id);
        }

        public static string Log = "";
        public clsTransaction()
        {
            ID = -1;
        }
        public clsTransaction(clsTransaction t)
        {
            ID = t.ID;
            Amount = t.Amount;
            Date = t.Date;
            Type = t.Type;
            CategoryID = t.CategoryID;
            WalletID = t.WalletID;
            ToWalletID = t.ToWalletID;
            Saved = t.Saved;
        }
        public static async Task<List<clsTransaction>?> GetLast10Obj()
        {
            return await clsTransactionData.GetLast10Obj();
        }
        public static async Task<List<clsTransaction>?> GetTransactonsInYear(DateTime DT)
        {
            var firstDayOfYear = new DateTime(DT.Year, 1, 1);
            var lastDayOfYear = firstDayOfYear.AddYears(1).AddSeconds(-1);

            return await clsTransactionData.GetAllOrderByDate(firstDayOfYear, lastDayOfYear);            
        }
        public static async Task<List<clsTransaction>?> GetTransactonsInMonth(DateTime DT)
        {
            var firstDayOfMonth = new DateTime(DT.Year, DT.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);            
            
            return await clsTransactionData.GetAllOrderByDate(firstDayOfMonth, lastDayOfMonth);
        }        
        public static async Task<List<clsTransaction>?> GetTransactonsInWeek(DateTime DT)
        {
            DateTime dt = new DateTime(DT.Year, DT.Month, DT.Day);
            
            var firstDayOfWeek = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            var lastDayOfWeek = dt.AddDays(-(int)dt.DayOfWeek + 8).AddSeconds(-1);            
            
            return await clsTransactionData.GetAllOrderByDate(firstDayOfWeek, lastDayOfWeek);
        }
        public static async Task<List<clsTransaction>?> GetTransactonsInDay(DateTime DT)
        {
            var firstHour = new DateTime(DT.Year, DT.Month, DT.Day);
            var lastHour = firstHour.AddDays(1).AddSeconds(-1);

            return await clsTransactionData.GetAllOrderByDate(firstHour, lastHour);
        }
        public static async Task<float> GetSumAmountByDate(DateTime DT, int AnalysisBy, int AnalysisType)
        {
            List<clsTransaction>? list = null;
            switch (AnalysisBy)
            {
                case 0:
                    list = await GetTransactonsInDay(DT);
                    break;
                case 1:
                    list = await GetTransactonsInWeek(DT);
                    break;
                case 2:
                    list = await GetTransactonsInMonth(DT);
                    break;
                case 3:
                    list = await GetTransactonsInYear(DT);
                    break;
            }
            if (list == null || list.Count < 0) 
                return 0;
            
            return list.Where((t) => t.Type == AnalysisType).Sum((t) => t.Amount);
        }
        public static async Task<List<clsTransaction>?> GetExpensesGroupByCategory(DateTime start, DateTime end )
        {
            return await clsTransactionData.GetExpensesGroupByCategory(start, end);
        }
        public static async Task<List<clsTransaction>?> GetIncomeGroupByCategory(DateTime start, DateTime end)
        {
            return await clsTransactionData.GetIncomeGroupByCategory(start, end);
        }

        public event Action<clsTransaction>? Saved;
    }
}
