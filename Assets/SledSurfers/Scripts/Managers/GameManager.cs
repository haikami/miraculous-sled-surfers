using System;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay;
using SledSurfers.Scripts.Gameplay.Level;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameplayManager _gameplayManager;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private PlayerManager _playerManager;
        
        [Header("UI")] 
        [SerializeField] private GameObject _playButton;
        
        private DataManager _dataManager;

        private void Awake()
        {
            _dataManager = ServiceLocator.Get<DataManager>();
            ServiceLocator.Get<LevelManager>().OnLevelLoaded += OnLevelLoaded;
        }

        private void OnEnable()
        {
            _gameplayManager.OnGameFinished += FinishGame;
        }

        private void OnDisable()
        {
            _gameplayManager.OnGameFinished -= FinishGame;
        }


        public void StartGame()
        {
            _playButton.SetActive(false);
            _gameplayManager.StartGame();
        }

        private void FinishGame(FinishReason reason)
        {
            _dataManager.SaveAsync();
            if (reason == FinishReason.ReachedEnd)
            {
                //TODO: implement me
            }
            else
            {
                ServiceLocator.Get<LevelManager>().ResetCurrentLevel();
            }
        }

        private void OnLevelLoaded()
        {
            if (!ServiceLocator.TryGet(out LevelDefinition levelDefinition))
            {
                Debug.LogError("No level definition found, add one to level scene");
                return;
            }

            var spawnPoint = levelDefinition.PlayerSpawnPoint;
            _cameraController.ToMainMenuView(spawnPoint);
            _playerManager.SetupPlayer(spawnPoint);
            _playButton.SetActive(true);
        }
    }
}