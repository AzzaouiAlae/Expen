using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Expen.clsUtility;

namespace Expen
{
    class clsCategoriesData
    {
        async static Task Init()
        {
            if (DB == null)
                DB = new(DatabasePath, flags);

            var table = await DB.CreateTableAsync<clsCategories>();
        }
        public async static Task<bool> Add(clsCategories Categorie)
        {
            await Init();
            int Result = await DB.InsertAsync(Categorie);
            return Result > 0;
        }
        public async static Task<bool> Update(clsCategories Categorie)
        {
            await Init();
            int Result = await DB.UpdateAsync(Categorie);
            return Result > 0;
        }
        public static async Task<bool> Delete(clsCategories Categorie)
        {
            await Init();
            int Result = await DB.DeleteAsync(Categorie);
            return Result > 0;
        }
        public static async Task<List<clsCategories>?> GetAll()
        {
            await Init();
            var Categories = await DB.QueryAsync<clsCategories>($"Select * from [clsCategories]");
            return Categories;
        }
        public static async Task<clsCategories?> Find(int id)
        {
            await Init();
            var Category = await DB.QueryAsync<clsCategories>($"Select * from [clsCategories] where [ID] = {id}");
            if(Category != null &&  Category.Count > 0)
                return Category[0];
            return null;
        }
        public static async Task<bool> Find(string name)
        {
            await Init();
            var Category = await DB.QueryAsync<clsCategories>($"Select * from [clsCategories] where [Name] = '{name}'");
            if (Category != null && Category.Count > 0)
                return true;

            return false;
        }
        public static async Task<clsCategories?> FindByName(string name)
        {
            await Init();
            var Category = await DB.QueryAsync<clsCategories>($"Select * from [clsCategories] where [Name] = '{name}'");
            if (Category != null && Category.Count > 0)
                return Category[0];

            return null;
        }
        public static async Task<int> Count()
        {
            await Init();
            var Category = await DB.QueryScalarsAsync<int>("Select count(ID) from [clsCategories]");
            int result = 0;
            if (Category != null)
                result = Category[0];


            return result;
        }
        public static async Task<List<clsCategories>?> GetAllByType(int Type)
        {
            await Init();
            var Categories = await DB.QueryAsync<clsCategories>($"Select * from [clsCategories] where [Type] = {Type}");
            return Categories;
        }
    }
}
