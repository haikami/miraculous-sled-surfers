using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Level Progression", fileName = "LevelProgressionConfig")]
    public class LevelProgressionConfig : ScriptableObject
    {
        [Header("Total number of levels available, levels must be in build settings and have the naming Level_x")]
        [SerializeField] private int _totalLevels = 1;

        public int TotalLevels => _totalLevels;
        
        public bool IsValidLevel(int level) => _totalLevels > level;

        //Cycle through levels, it could be changed to just loading the last level over and over or any other logic
        public int GetNextLevelIndex(int currentLevelIndex) => (currentLevelIndex + 1) % _totalLevels;
    }
}