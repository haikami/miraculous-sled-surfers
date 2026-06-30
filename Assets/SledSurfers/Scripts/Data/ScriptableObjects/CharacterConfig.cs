using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Player/Visual config", fileName = "VisualConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [SerializeField] private CharacterType _type;
        [SerializeField] private GameObject _characterVisualPrefab;
        //For now I don't have a menu for character selection but adding for future iterations
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _portrait;

        public GameObject CharacterVisualPrefab => _characterVisualPrefab;
        public string DisplayName => _displayName;
        public Sprite Portrait => _portrait;
        public CharacterType Type => _type;
    }

    public enum CharacterType
    {
        Lady = 0,
        AstroCat =1
    }
}