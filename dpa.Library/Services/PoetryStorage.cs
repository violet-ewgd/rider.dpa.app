using System.Linq.Expressions;
using dpa.Library.Helpers;
using dpa.Library.Models;
using SQLite;

namespace dpa.Library.Services;

public class PoetryStorage : IPoetryStorage
{
    public const int NumberPoetry = 30;
    
    public const string DbName = "poetrydb.sqlite3";
    
    public static readonly string PoetryDbPath = 
        PathHelper.GetLocalFilePath(DbName);

    private SQLiteAsyncConnection _connection;
    
    private SQLiteAsyncConnection Connection => 
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);
    
    private readonly IPreferenceStorage _preferenceStorage;

    public bool IsInitialized =>
        _preferenceStorage.Get(PoetryStorageConstant.VersionKey,
            default(int)) == PoetryStorageConstant.Version;
    
// yilaizhuru
    public PoetryStorage(IPreferenceStorage preferenceStorage)
    {
        _preferenceStorage = preferenceStorage;
    }
    
// static class  use for package or helpers    
    public static class PoetryStorageConstant {
        public const int Version = 1;
        
        public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
        
    }
    
    public async Task InitializeAsync()
    {   //来源流
        await using var dbFileStream =
               new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
        //目标流  
        await using var dbAssetStream =
            typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName);
        //流对流拷贝
        await dbAssetStream.CopyToAsync(dbFileStream);
        
        _preferenceStorage.Set(PoetryStorageConstant.VersionKey,
            PoetryStorageConstant.Version);
    }
    
    public async Task<Poetry> GetPoetryAsync(int id) =>
        await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);


    public async Task<IList<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        var table = Connection.Table<Poetry>();
        await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take)
        .ToListAsync();
        return await table.Where(where).Skip(skip).Take(take).ToListAsync();
    }
    
    public async Task CloseAsync() => await Connection.CloseAsync();
    
}