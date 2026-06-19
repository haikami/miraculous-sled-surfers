using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Providers;
using SledSurfers.Scripts.Extensions;
using SledSurfers.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SledSurfers.Scripts.Core
{
    
    public class Bootstrap : MonoBehaviour
    {
        private const string GameCoreSceneName = "GameCore";
        
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private bool _loadFromServer; 

        private async void Awake()
        {
            await Run();
        }

        private async Task Run()
        {
            _loadingScreen.Show();
            RegisterServices();
            await SceneManager.LoadSceneAsync(GameCoreSceneName, LoadSceneMode.Additive).AsTask();
            ServiceLocator.Get<LoadingScreen>()?.Show();
            await FetchPlayerData();
            await LoadScenes();
            _loadingScreen.Hide();

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
        

        private void RegisterServices()
        {
            DontDestroyOnLoad(_loadingScreen);
            ServiceLocator.Register(_loadingScreen);
            
            var levelManager = new LevelManager();
            var gameStateManager = new GameStateManager();
            
            ServiceLocator.Register(levelManager);
            ServiceLocator.Register(gameStateManager);
            
            var provider = BuildPlayerDataProvider();
            var dataManager = new DataManager(provider);
            ServiceLocator.Register(dataManager);

            var currencyManager = new CurrencyManager(dataManager);
            ServiceLocator.Register(currencyManager);
            
            gameStateManager.Initialize();
        }
        

        private async Task FetchPlayerData() => await ServiceLocator.Get<DataManager>().LoadAsync();

        private IPlayerDataProvider BuildPlayerDataProvider()
        {
            var local  = new LocalPlayerDataProvider();
            if (!_loadFromServer)
            {
                return local;
            }
            
            var server = new ServerPlayerDataProvider(timeout: TimeSpan.FromSeconds(5));
            return new PlayerDataProviderWithFallback(server, local);
        }

        private async Task LoadScenes()
        {
            var levelManager = ServiceLocator.Get<LevelManager>();
            var targetLevel  = ServiceLocator.Get<DataManager>().PlayerData.currentLevel;
            
            await levelManager.LoadLevelAsync(targetLevel);
        }
    }
}