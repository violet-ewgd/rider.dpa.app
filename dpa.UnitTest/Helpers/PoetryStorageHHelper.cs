using dpa.Library.Services;

namespace dpa.UnitTest.Helpers;

public class PoetryStorageHHelper
{
    public static void RemoveDatabaseFile() => 
          File.Delete(PoetryStorage.PoetryDbPath);

}