using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Extensions;
using SledSurfers.Scripts.Gameplay.Collectables;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace SledSurfers.Scripts.Managers
{
    public class LevelManager
    {
        public event Action OnLevelLoaded;

        private string _currentLevelScene;
        
        private static string GetSceneName(int index) => $"Level_{index}";

        private bool IsInitialLoad => string.IsNullOrWhiteSpace(_currentLevelScene);
        
        public async Task LoadLevelAsync(int levelIndex)
        {
            var targetScene = GetSceneName(levelIndex);
            if (!IsInitialLoad)
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
            OnLevelLoaded?.Invoke();
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
            OnLevelLoaded?.Invoke();
        }
    }
}