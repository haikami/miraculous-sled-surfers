using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.UI.Meta
{
    public class MainMenuView : MonoBehaviour
    {
        private readonly Lazy<GameStateManager> _gameManager = new(ServiceLocator.Get<GameStateManager>);
        private GameStateManager GameStateManager => _gameManager.Value;
        
        private void Awake()
        {
            GameStateManager.OnStateChanged += OnGameStateChanged;
            OnGameStateChanged(GameStateManager.GameState);
        }

        private void OnDestroy()
        {
            if (GameStateManager != null)
            {
                GameStateManager.OnStateChanged -= OnGameStateChanged;
            }
        }

        private void Hide() => gameObject.SetActive(false);
        private void Show() => gameObject.SetActive(true);

        private void OnGameStateChanged(GameState newState)
        {
            if (newState == GameState.MainMenu)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }


        public void OnStartPressed()
        => ServiceLocator.Get<GameManager>()?.StartGame();
    }
}