
using SQLite;
using System.IO;

namespace Expen;

public class clsUtility 
{
	static public string DatabaseFileName = "myDB.db3";

	static public SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
	static public string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);	

    static public SQLiteAsyncConnection DB;
}