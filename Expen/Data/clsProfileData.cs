using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Expen.clsUtility;

namespace Expen
{
    internal class clsProfileData
    {        
        async static Task Init()
        {
            if (DB == null)
                DB = new(DatabasePath, flags);

            var table = await DB.CreateTableAsync<clsProfile>();
        }
        static public async Task<bool> isProfileExists()
        {
            await Init();
            var profile = await DB.QueryAsync<clsProfile>($"Select * from [clsProfile] where [ID] = 1");

            if (profile != null && profile.Count > 0)
                return true;
            else
                return false;
        }
        static public async Task<bool> Save(clsProfile profile)
        {
            bool Result = await isProfileExists();
            if (Result)
                return await Update(profile);            
            else
                return await Add(profile);
        }
        static public async Task<bool> Add(clsProfile profile)
        {
            await Init();
            int Result = await DB.InsertAsync(profile);
            return Result > 0;
        }
        static public async Task<bool> Update(clsProfile profile)
        {
            await Init();
            int Result = await DB.UpdateAsync(profile);
            return Result > 0;
        }
        static public async Task<clsProfile?> Find(int ProfileID)
        {
            await Init();
            var profile = await DB.QueryAsync<clsProfile>($"Select * from [clsProfile] where [ID] = 1");

            if (profile != null && profile.Count > 0)
                return profile[0];
            else
                return null;
        }
    }
}
