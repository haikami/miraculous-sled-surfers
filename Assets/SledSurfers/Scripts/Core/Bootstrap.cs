using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Providers;
using SledSurfers.Scripts.Data.ScriptableObjects;
using SledSurfers.Scripts.Extensions;
using SledSurfers.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SledSurfers.Scripts.Core
{
    
    public class Bootstrap : MonoBehaviour
    {
        private const string GameCoreSceneName = "GameCore";
        
        [Header("Configs")]
        [SerializeField] private UpgradeListConfig _upgradeListConfig;
        
        [Header("References")]
        [SerializeField] private LoadingScreen _loadingScreen;
        
        [Header("Debug")]
        [SerializeField] private bool _loadFromServer; 

        private async void Awake()
        {
            await Run();
        }

        private async Task Run()
        {
            _loadingScreen.Show();
            DontDestroyOnLoad(_loadingScreen);
            
            RegisterServices();
            await FetchPlayerData();
            await SceneManager.LoadSceneAsync(GameCoreSceneName, LoadSceneMode.Additive).AsTask();
            await LoadCurrentLevel();
            _loadingScreen.Hide();

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
        

        private void RegisterServices()
        {
            ServiceLocator.Register(_loadingScreen);
            
            var levelManager = new LevelManager();
            ServiceLocator.Register(levelManager);
            
            var gameStateManager = new GameStateManager();
            ServiceLocator.Register(gameStateManager);
            
            var provider = BuildPlayerDataProvider();
            var dataManager = new DataManager(provider);
            ServiceLocator.Register(dataManager);

            var currencyManager = new CurrencyManager(dataManager);
            ServiceLocator.Register(currencyManager);

            var upgradesManager = new UpgradesManager(_upgradeListConfig, currencyManager, dataManager);
            ServiceLocator.Register(upgradesManager);
            
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

        private async Task LoadCurrentLevel()
        {
            var levelManager = ServiceLocator.Get<LevelManager>();
            var targetLevel  = ServiceLocator.Get<DataManager>().PlayerData.currentLevel;
            
            await levelManager.LoadLevelAsync(targetLevel);
        }
    }
}