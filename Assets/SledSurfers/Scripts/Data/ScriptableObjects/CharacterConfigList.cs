using System.Collections.Generic;
using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    
    [CreateAssetMenu(menuName = "Config/Player/Visual list", fileName = "AvailableCharactersConfig")]
    public class CharacterConfigList : ScriptableObject
    {
        [SerializeField] private List<CharacterConfig> _characterConfigs;
        
        public CharacterConfig DefaultCharacterConfig => _characterConfigs[0];
        public int NumConfigs => _characterConfigs.Count;
        
        //Not very useful outside of the cheat used to cycle between characters
        public CharacterConfig GetNextCharacterConfig(CharacterConfig current)
        {
            var currentIndex = _characterConfigs.IndexOf(current);
            if (currentIndex == -1)
            {
                return DefaultCharacterConfig;
            }
            return _characterConfigs[(currentIndex+1) % NumConfigs];
        }
        
        
        public CharacterConfig GetCharacterOrDefault(CharacterType type)
        =>  _characterConfigs.Find(x => x.Type == type) ?? DefaultCharacterConfig;
    }
}