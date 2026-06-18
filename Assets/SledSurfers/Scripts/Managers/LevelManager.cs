using System;
using System.Threading.Tasks;
using SledSurfers.Scripts.Extensions;
using UnityEngine.SceneManagement;

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

        //TODO: cleanup whatever is needed
        public void ResetCurrentLevel()
        {
            var scene   = SceneManager.GetSceneByName(_currentLevelScene);
            OnLevelLoaded?.Invoke();
        }
    }
}