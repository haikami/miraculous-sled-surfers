using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Obstacles
{
    public enum ObstacleType
    {
        Soft,   // e.g. small rock, bush — grazeable, momentum penalty only
        Hard    // e.g. tree, boulder — always a crash regardless of angle
    }

    public class ObstacleData : MonoBehaviour
    {
        [SerializeField] private ObstacleType _obstacleType;
        [SerializeField] private float _momentumPenaltyOverride = -1f; // -1 = use config default

        public ObstacleType ObstacleType => _obstacleType;
        public float MomentumPenaltyOverride => _momentumPenaltyOverride;
    }
}