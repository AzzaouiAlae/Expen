using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expen
{
    public class clsProfile
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; } = 1;
        public string Name { get; set; } = "";
        public string imgURL { get; set; } = FileSystem.AppDataDirectory + "\\image.jpg";
        public static async Task<bool> isProfileExists()
        {
            return await clsProfileData.isProfileExists();
        }
        public async Task<bool> Save()
        {
            return await clsProfileData.Save(this);
        }
        public clsProfile()
        {

        }
        public clsProfile(int id, Action load)
        {
            LoadProfile(id, load);
        }
        async void LoadProfile(int id, Action load)
        {
            clsProfile? p = await clsProfileData.Find(id);
            if(p != null)
            {
                ID = p.ID;
                Name = p.Name;
                imgURL = p.imgURL;
                load.Invoke();
            }
        }
    }
}
