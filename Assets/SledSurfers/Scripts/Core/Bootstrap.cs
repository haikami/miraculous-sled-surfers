using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Cheats;
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

        [SerializeField] private LevelProgressionConfig _levelProgressionConfig;
        
        [Header("References")]
        [SerializeField] private LoadingScreen _loadingScreen;
        
        [Header("Debug")]
        [SerializeField] private bool _loadFromServer;

        [SerializeField] private bool _enableCheats = true;
        [SerializeField] private CheatsMenu _cheatsMenuInstance;

        private async void Awake()
        {
            await Run();
        }

        private async Task Run()
        {
            _loadingScreen.Show();
            DontDestroyOnLoad(_loadingScreen);

            if (_enableCheats)
            {
                SetupCheatsMenu();
            }
            
            RegisterServices();
            await FetchPlayerData();
            await SceneManager.LoadSceneAsync(GameCoreSceneName, LoadSceneMode.Additive).AsTask();
            await LoadCurrentLevel();
            _loadingScreen.Hide();

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private void SetupCheatsMenu()
        {
            var cheatsMenu = Instantiate(_cheatsMenuInstance);
            DontDestroyOnLoad(cheatsMenu);
            ServiceLocator.Register(cheatsMenu);
        }
        

        private void RegisterServices()
        {
            ServiceLocator.Register(_loadingScreen);
            
            var gameStateManager = new GameStateManager();
            ServiceLocator.Register(gameStateManager);
            
            var gameplayStateManager = new GameplayStateManager();
            ServiceLocator.Register(gameplayStateManager);
            
            var levelManager = new LevelManager();
            ServiceLocator.Register(levelManager);
            
            var provider = BuildPlayerDataProvider();
            var dataManager = new DataManager(provider, _levelProgressionConfig);
            ServiceLocator.Register(dataManager);

            var currencyManager = new CurrencyManager(dataManager);
            ServiceLocator.Register(currencyManager);

            var upgradesManager = new UpgradesManager(_upgradeListConfig, currencyManager, dataManager);
            ServiceLocator.Register(upgradesManager);
            
            ServiceLocator.Register(new RunResultManager());
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