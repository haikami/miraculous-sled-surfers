using System.Threading.Tasks;
using SledSurfers.Scripts.Events;
using SledSurfers.Scripts.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SledSurfers.Scripts.Managers
{
    // Managers/LevelManager.cs
    public class LevelManager
    {
        private readonly GameStateChannel _gameStateChannel;
        private readonly LevelLoadedChannel _levelLoadedChannel;

        private string _currentLevelScene;

        // convention: level scenes named "Level_01", "Level_02", etc.
        private static string GetSceneName(int index) => $"Level_{index}";

        public LevelManager(
            GameStateChannel gameStateChannel,
            LevelLoadedChannel levelLoadedChannel)
        {
            _gameStateChannel    = gameStateChannel;
            _levelLoadedChannel  = levelLoadedChannel;
        }

        public async Task LoadLevelAsync(int levelIndex)
        {
            var targetScene = GetSceneName(levelIndex);

            if (!string.IsNullOrWhiteSpace(_currentLevelScene))
            {
                if (_currentLevelScene == targetScene)
                {
                    ResetCurrentLevel();
                    return;
                }

                await UnloadCurrentLevelAsync();
            }

            await LoadLevelSceneAsync(targetScene);
        }

        private async Task LoadLevelSceneAsync(string sceneName)
        {
            _currentLevelScene = sceneName;
            await SceneManager.LoadSceneAsync(_currentLevelScene, LoadSceneMode.Additive).AsTask();
            _levelLoadedChannel.Raise();
        }

        private async Task UnloadCurrentLevelAsync()
        {
            await SceneManager.UnloadSceneAsync(_currentLevelScene).AsTask();
            _currentLevelScene = null;
        }

        private void ResetCurrentLevel()
        {
            var scene   = SceneManager.GetSceneByName(_currentLevelScene);
            // var resetter = scene.GetRootGameObjects()
            //     .Select(go => go.GetComponent<LevelResetter>())
            //     .FirstOrDefault(r => r != null);
            //
            // if (resetter == null)
            // {
            //     Debug.LogWarning($"[LevelManager] No LevelResetter found in {_currentLevelScene}");
            //     return;
            // }
            //
            // resetter.Reset();
            _levelLoadedChannel.Raise();
        }
    }
}