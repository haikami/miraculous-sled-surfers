using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.UI.Meta
{
    public class MainMenuView : MonoBehaviour
    {
        private readonly Lazy<GameManager> _gameManager = new(ServiceLocator.Get<GameManager>);
        private GameManager GameManager => _gameManager.Value;
        
        private void Awake()
        {
            GameManager.OnStateChanged += OnGameStateChanged;
            OnGameStateChanged(GameManager.GameState);
        }

        private void OnDestroy()
        {
            if (GameManager != null)
            {
                GameManager.OnStateChanged -= OnGameStateChanged;
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