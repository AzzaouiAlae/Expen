using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expen
{
    public class clsWallet
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; } 
        public string Type { get; set; }
        public string Name { get; set; }
        public float Balance { get; set; }
        public string color { get; set; }
        public string imgURL { get; set; }
        public DateTime Date { get; set; }
        public clsWallet()
        {
            ID = -1;
        }
        public clsWallet(clsWallet w)
        {
            ID = w.ID;
            Type = w.Type;
            Name = w.Name;
            Balance = w.Balance;
            color = w.color;
            imgURL = w.imgURL;
            Saved = w.Saved;
        }
        public static async Task<List<clsWallet>?> GetAll()
        {
            return await clsWalletData.GetAll();
        }
        public async Task<bool> Save()
        {
            bool Result = false;
            if(ID == -1)
                Result = await clsWalletData.Add(this);
            else
                Result = await clsWalletData.Update(this);

            if (Result)
                Saved?.Invoke(this);
            return Result;
        }
        public static async Task<bool> Delete(clsWallet wallet)
        {
            return await clsWalletData.Delete(wallet);
        }
        public async Task<bool> Withdrawal(float Amount , bool isUndoTransaction = false)
        {
            if (!isUndoTransaction && Balance < Amount)
            {
                clsTransaction.Log = "failed to Withdrawal Amount";
                return false;
            }

            Balance -= Amount;

            bool Result = await Save();

            if (!Result)
                clsTransaction.Log = "failed to Withdrawal Amount";

            return Result;
        }
        public async Task<bool> Deposit(float Amount)
        {
            Balance += Amount;
            bool Result = await Save();

            if (!Result)
                clsTransaction.Log = "failed to Deposit Amount";            
            return Result;
        }
        public static async Task<clsWallet?> Find(int id)
        {
            return await clsWalletData.Find(id);
        }
        public async Task<bool> Delete()
        {
            return await Delete(this);
        }

        public event Action<clsWallet>? Saved;

    }
}