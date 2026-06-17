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

            RegisterManagers();

            await FetchPlayerData();
            await LoadScenes();

            _loadingScreen.Hide();
        }
        

        private void RegisterManagers()
        {
            var levelManager = new LevelManager();
            var gameStateManager = new GameManager();
            
            ServiceLocator.Register(levelManager);
            ServiceLocator.Register(gameStateManager);
            //
            // // audio etc — no channels needed at construction
            // ServiceLocator.Register<AudioManager>(new AudioManager());
            
            var provider = BuildPlayerDataProvider();
            ServiceLocator.Register(new DataManager(provider));
            
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
            int targetLevel  = ServiceLocator.Get<DataManager>().PlayerData.CurrentLevel;
            
            await SceneManager.LoadSceneAsync(MetaSceneName, LoadSceneMode.Additive).AsTask();
            await levelManager.LoadLevelAsync(targetLevel);
        }
    }
}