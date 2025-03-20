using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expen
{
    public class clsCategories
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; } = -1;
        public string Name { get; set; }
        public string Description { get; set; }
        public string imgURL { get; set; }
        public string color { get; set; }
        public byte Type { get; set; } //0 = Expense | 1 = Income | 2 = Transfer
        public clsCategories()
        {
            Name = "";
            Description = "";
            imgURL = "";
            color = "";
        }
    [Ignore]
        public float Total 
        { 
            get{ return 0; }
        }
        public async Task<bool> Save()
        {
            if (ID == -1)
                return await clsCategoriesData.Add(this);
            else
                return await clsCategoriesData.Update(this);
        }
        public async static Task<bool> Delete(int id)
        {
            return await clsCategoriesData.Delete(new clsCategories() { ID = id});
        }
        public static async Task<List<clsCategories>?> GetAll()
        {
            return await clsCategoriesData.GetAll();
        }
        public static async Task<clsCategories?> Find(int id)
        {
            return await clsCategoriesData.Find(id);
        }
        public static async Task<clsCategories?> Find(string Name)
        {
            return await clsCategoriesData.FindByName(Name);
        }
        static public async Task<int> Count()
        {
            return await clsCategoriesData.Count();
        }
        static public async Task FillDefault()
        {
            int result = await Count();
            if(result < 19)
            {
                List<clsCategories> Default = new();
                Default.Add(new clsCategories() { Name = "Food & Drink", Description = "", color = "#E8750B", imgURL = "food.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Transport", Description = "", color = "#233288", imgURL = "transport.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Shopping", Description = "", color = "#DBC101", imgURL = "shopping.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Pet", Description = "", color = "#FA84EE", imgURL = "pet.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Home", Description = "", color = "#D9665A", imgURL = "home.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Children", Description = "", color = "#FFAE64", imgURL = "children.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Health", Description = "", color = "#1B9EFD", imgURL = "health.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Contact", Description = "", color = "#03427B", imgURL = "contact.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Sport", Description = "", color = "#143618", imgURL = "sport.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Order", Description = "", color = "#ACDF1C", imgURL = "order.png", Type = 0 });
                Default.Add(new clsCategories() { Name = "Transfer", Description = "", color = "#F99", imgURL = "transfer.png", Type = 2 });


                Default.Add(new clsCategories() { Name = "Salary", Description = "", color = "#E8750B", imgURL = "salary.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Business Profits", Description = "", color = "#233288", imgURL = "business_profits.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Investments", Description = "", color = "#DBC101", imgURL = "investments.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Rental Income", Description = "", color = "#FA84EE", imgURL = "rental_income.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Freelancing", Description = "", color = "#D9665A", imgURL = "freelancing.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Gifts", Description = "", color = "#FFAE64", imgURL = "gifts.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Sale of Assets", Description = "", color = "#1B9EFD", imgURL = "sale_of_assets.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Pension", Description = "", color = "#03427B", imgURL = "pension.png", Type = 1 });
                Default.Add(new clsCategories() { Name = "Government Benefits", Description = "", color = "#143618", imgURL = "government_benefits.png", Type = 1 });
                


                foreach (var item in Default)
                {
                    bool Result = await clsCategoriesData.Find(item.Name);
                    if (!Result)
                    {
                        await item.Save();
                    }
                    else if(item.Name == "Transfer")
                    {
                        await item.Save();
                    }
                }
            }
        }
        public static async Task<List<clsCategories>?> GetAllByType(int Type)
        {
            return await clsCategoriesData.GetAllByType(Type);
        }
    }
}
