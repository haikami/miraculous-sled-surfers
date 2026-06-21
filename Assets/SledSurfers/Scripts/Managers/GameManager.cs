using System;
using SledSurfers.Scripts.Cheats;
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
        
        private DataManager _dataManager;
        private GameStateManager _gameStateManager;
        private RunResultManager _runResultManager;
        private CurrencyManager _currencyManager;
        private LevelManager _levelManager;
        private UpgradesManager _upgradesManager;

        private void Awake()
        {
            _dataManager = ServiceLocator.Get<DataManager>();
            _gameStateManager = ServiceLocator.Get<GameStateManager>();
            _runResultManager = ServiceLocator.Get<RunResultManager>();
            _currencyManager = ServiceLocator.Get<CurrencyManager>();
            _levelManager = ServiceLocator.Get<LevelManager>();
            _upgradesManager = ServiceLocator.Get<UpgradesManager>();
            
            //Example on how to dynamically add cheats, it will only happen if menu is instantiated during preload
            if (ServiceLocator.TryGet(out CheatsMenu cheatsMenu))
            {
                cheatsMenu.AddCheat("Load Next Level", LoadNextLevel);
            }
            
            _levelManager.OnLevelLoaded += OnLevelLoaded;
        }

        public void StartGame()
        {
            _gameplayManager.StartGame();
            _gameStateManager.SwitchState(GameState.Playing);
        }

        public void FinishGame()
        {
            var runResult = _runResultManager.LastRunResultData;
            _runResultManager.Clear();
            _currencyManager.Add(runResult.currencies);
            
            if (runResult.reason == FinishReason.ReachedEnd)
            {
                LoadNextLevel();
            }
            else
            {
                _dataManager.UpdateMaxDistanceIfHigher(runResult.distanceTraveled);
                _dataManager.SaveAsync();
                _levelManager.ResetCurrentLevel();
            }
        }

        private void LoadNextLevel()
        {
            _dataManager.SetNextLevelData();
            _currencyManager.ResetLevelCurrencies();
            _upgradesManager.ResetUpgrades();
            _dataManager.SaveAsync();
            //Fire and forget since loading level triggers and event
            _levelManager.LoadLevelAsync(_dataManager.CurrentLevel);
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
            _gameStateManager.SwitchState(GameState.MainMenu);
        }
    }
}