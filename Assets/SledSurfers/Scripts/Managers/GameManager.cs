using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay.Level;
using SledSurfers.Scripts.Player;
using UnityEngine;

namespace SledSurfers.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] 
        private CameraController _cameraController;

        [SerializeField] 
        private PlayerManager _playerManager;


        private void Awake()
        {
            ServiceLocator.Get<LevelManager>().OnLevelLoaded += SetElements;
            ServiceLocator.Get<GameStateManager>().OnStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Playing:
                {
                    _cameraController.ToIdleView();
                }
                    break;
                default:
                    break;
            }
        }

        private void SetElements()
        {
            if (!ServiceLocator.TryGet(out LevelDefinition levelDefinition))
            {
                Debug.LogError("No level definition found, add one to level scene");
                return;
            }

            var spawnPoint = levelDefinition.PlayerSpawnPoint;
            _cameraController.ToMainMenuView(spawnPoint);
            _playerManager.ResetPlayer(spawnPoint);
        }
    }
}