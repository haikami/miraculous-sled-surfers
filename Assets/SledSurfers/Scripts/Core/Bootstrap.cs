using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Data.Models;
using SledSurfers.Scripts.Data.Providers;
using SledSurfers.Scripts.Events;
using SledSurfers.Scripts.Extensions;
using SledSurfers.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SledSurfers.Scripts.Core
{
    
    public class Bootstrap : MonoBehaviour
    {
        private const string MetaSceneName = "Meta";
        
        [SerializeField] private bool _loadFromServer; 
        [SerializeField] private LoadingScreen _loadingScreen;

        private async void Awake()
        {
            await Run();
        }

        private async Task Run()
        {
            _loadingScreen.Show();

            InitEvents();
            InitManagers();

            await FetchPlayerData();
            await LoadScenes();

            _loadingScreen.Hide();
        }

        private void InitEvents()
        {
            var gameStateChannel = new GameStateChannel();
            var runChannel       = new RunChannel();
            var currencyChannel  = new CurrencyChannel();
            var levelChannel     = new LevelLoadedChannel();

            // subscribers get interface only
            ServiceLocator.Register<IChannel<GameState>>(gameStateChannel);
            ServiceLocator.Register<IChannel<RunResultData>>(runChannel);
            ServiceLocator.Register<IChannel<int>>(currencyChannel);
            ServiceLocator.Register<IChannel>(levelChannel);

            // owners get concrete — stored separately so locator never exposes Raise()
            ServiceLocator.Register<GameStateChannel>(gameStateChannel);
            ServiceLocator.Register<RunChannel>(runChannel);
            ServiceLocator.Register<CurrencyChannel>(currencyChannel);
            ServiceLocator.Register<LevelLoadedChannel>(levelChannel);
        }

        private void InitManagers()
        {
            // var gameStateManager = new GameStateManager(
            //     ServiceLocator.Get<GameStateChannel>()
            // );
            // var currencyManager = new CurrencyManager(
            //     ServiceLocator.Get<CurrencyChannel>()
            // );
            var levelManager = new LevelManager(
                ServiceLocator.Get<GameStateChannel>(),
                ServiceLocator.Get<LevelLoadedChannel>()
            );
            //
            // ServiceLocator.Register<GameStateManager>(gameStateManager);
            // ServiceLocator.Register<CurrencyManager>(currencyManager);
            ServiceLocator.Register<LevelManager>(levelManager);
            //
            // // audio etc — no channels needed at construction
            // ServiceLocator.Register<AudioManager>(new AudioManager());
            
            var provider = BuildPlayerDataProvider();
            var dataManager = new DataManager(provider);
            ServiceLocator.Register<DataManager>(dataManager);
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
            int targetLevel  = ServiceLocator.Get<DataManager>().PlayerData.CurrentLevel;
            
            await SceneManager.LoadSceneAsync(MetaSceneName, LoadSceneMode.Additive).AsTask();
            await levelManager.LoadLevelAsync(targetLevel);
        }
    }
}