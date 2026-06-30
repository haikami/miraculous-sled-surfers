using UnityEngine;
using SledSurfers.Scripts.Data.ScriptableObjects;

namespace SledSurfers.Scripts.Player
{
    public class PlayerCharacterLoader : MonoBehaviour
    {
        public CharacterConfig CurrentCharacterConfig {get; private set;}
        public PlayerAnimationController AnimationController { get; private set; }
        public PlayerRagdollController RagdollController { get; private set; }

        private GameObject _currentCharacterInstance;

        public void LoadCharacter(CharacterConfig config)
        {
            if (_currentCharacterInstance != null)
                Destroy(_currentCharacterInstance);

            _currentCharacterInstance = Instantiate(config.CharacterVisualPrefab, transform); // transform = CharacterMount
            CurrentCharacterConfig = config;
            AnimationController = _currentCharacterInstance.GetComponent<PlayerAnimationController>();
            RagdollController = _currentCharacterInstance.GetComponent<PlayerRagdollController>();
        }
    }
}