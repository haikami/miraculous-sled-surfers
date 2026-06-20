using SledSurfers.Scripts.Cheats;
using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Gameplay.UI;
using SledSurfers.Scripts.Managers;
using UnityEngine;

namespace SledSurfers.Scripts.UI
{
    public class MenuView : MonoBehaviour
    {
        [Header("UI elements")] 
        [SerializeField] private GameObject _mainMenuContainer;
        [SerializeField] private GameObject _gameMenuContainer;
        [SerializeField] private GameObject _quitButton;
        [SerializeField] private ResultView _resultsView;
        [SerializeField] private TopBarView _topBarView;

        [Header("References")] 
        [SerializeField] private GameManager _gameManager;

        public void StartClicked() => _gameManager.StartGame();
        public void FinishClicked() => _gameManager.FinishGame();

        private void HandleGameplayStateChanged(GameplayState state)
        {
            _quitButton.SetActive(state == GameplayState.Slingshot);
            _resultsView.gameObject.SetActive(state == GameplayState.GameOver);
        }
        
        private void HandleGameStateChanged(GameState state)
        {
            if (ServiceLocator.TryGet(out CheatsMenu cheatsMenu))
            {
                cheatsMenu.SetButtonVisibility(state == GameState.MainMenu);
            }
            
            _mainMenuContainer.SetActive(state == GameState.MainMenu);
            _gameMenuContainer.SetActive(state == GameState.Playing);
        }
        
        private void OnEnable()
        {
            var gameplayStateManager = ServiceLocator.Get<GameplayStateManager>();
            var stateManager = ServiceLocator.Get<GameStateManager>();

            stateManager.OnStateChanged += HandleGameStateChanged;
            gameplayStateManager.OnStateChanged += HandleGameplayStateChanged;
            
            HandleGameStateChanged(stateManager.CurrentState);
            HandleGameplayStateChanged(gameplayStateManager.CurrentState);
        }

        private void OnDisable()
        {
            ServiceLocator.Get<GameStateManager>().OnStateChanged -= HandleGameStateChanged;
            ServiceLocator.Get<GameplayStateManager>().OnStateChanged -= HandleGameplayStateChanged;
        }
    }
}