using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Slingshot", fileName = "SlingshotConfig")]
    public class SlingshotConfig : ScriptableObject
    {
        [Header("Shape")]
        [Tooltip("Max lateral distance from center — also the radius of the bottom semicircle")]
        [SerializeField] private float _maxLateralOffset = 1f;

        [Tooltip("Depth of the straight vertical section before the semicircle begins")]
        [SerializeField] private float _straightDepth = 1.5f;

        [Header("Force Percentage")]
        [Tooltip("Percentage at the very bottom center of the curve (max pull)")]
        [SerializeField] private float _maxPercentage = 1f;

        [Header("Launch")]
        [Tooltip("Minimum percentage required for a valid launch — below this, aiming is cancelled on release")]
        [SerializeField] private float _minimumLaunchPercentage = 0.2f;

        public float MaxLateralOffset => _maxLateralOffset;
        public float StraightDepth => _straightDepth;
        public float MaxPercentage => _maxPercentage;
        public float MinimumLaunchPercentage => _minimumLaunchPercentage;
    }
}