using System.Linq.Expressions;
using dpa.Library.Models;
using dpa.Library.Services;
using dpa.UnitTest.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;

namespace dpa.UnitTest.Services;

public class PoetryStorageTest: IDisposable
{
    //TestInitializeAsync
    //InitializeAsync_Success
    //InitializeAsync_Fail
    
    [Fact]
    public async Task GetPoetriesAsync_Default() {
        var poetryStorage =
            await PoetryStorageHelper.GetInitializedPoetryStorage();
        var poetries = await poetryStorage.GetPoetriesAsync(
            Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);
        //看读取的数量和设置的常量是否一致
        Assert.Equal(PoetryStorage.NumberPoetry, poetries.Count());
        await poetryStorage.CloseAsync();
    }
    
    [Fact]
    public async Task GetPoetryAsync_Default() {
        var poetryStorage = await PoetryStorageHelper
            .GetInitializedPoetryStorage();
        var poetry = await poetryStorage.GetPoetryAsync(10001);
        Assert.Equal("临江仙 · 夜归临皋", poetry.Name);
        //没关数据库
        await poetryStorage.CloseAsync();
    }
    
    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        preferenceStorageMock.Setup(p => p.Get(PoetryStorage.PoetryStorageConstant.VersionKey, default(int)))
            .Returns(PoetryStorage.PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);

        Assert.True(poetryStorage.IsInitialized);
        
        preferenceStorageMock.Verify(p =>p.Get(PoetryStorage.PoetryStorageConstant.VersionKey, default(int)),
            Times.Once);
    }
    
    
    
    [Fact]
    public async Task InitializeAsync_Default()

    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        

        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializeAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));


    }

    public void Dispose() {
        PoetryStorageHelper.RemoveDatabaseFile();
    }
    

}