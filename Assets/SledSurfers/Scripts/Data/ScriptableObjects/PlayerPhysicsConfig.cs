using UnityEngine;

namespace SledSurfers.Scripts.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Config/Player Physics", fileName = "PlayerPhysicsConfig")]
    public class PlayerPhysicsConfig : ScriptableObject
    {
        [Header("Mass & Drag")]
        [Tooltip("Rigidbody mass — higher feels heavier, slower to accelerate, harder to deflect")]
        [SerializeField] private float _mass = 1f;

        [Tooltip("Linear drag — natural deceleration over time")]
        [SerializeField] private float _linearDrag = 0.1f;

        [Tooltip("Angular drag — how quickly unwanted rotation settles")]
        [SerializeField] private float _angularDrag = 5f;
        

        [Header("Lateral / Steering")]
        [Tooltip("Base lateral force per unit of drag input")]
        [SerializeField] private float _baseLateralForce = 5f;

        [Tooltip("Max lateral speed achievable")]
        [SerializeField] private float _maxLateralSpeed = 3f;

        [Tooltip("Pixels needed for a full-strength steering input")]
        [SerializeField] private float _maxDragDistance = 200f;

        [Header("Slingshot Launch")]
        [Tooltip("Multiplier applied to pull percent to determine launch force")]
        [SerializeField] private float _baseLaunchForceMultiplier = 10f;

        [Header("Momentum / Run End")]
        [Tooltip("Speed below which the run ends due to lost momentum")]
        [SerializeField] private float _minimumSpeedThreshold = 1.5f;

        public float Mass => _mass;
        public float LinearDrag => _linearDrag;
        public float AngularDrag => _angularDrag;
        public float BaseLateralForce => _baseLateralForce;
        public float MaxLateralSpeed => _maxLateralSpeed;
        public float MaxDragDistance => _maxDragDistance;
        public float BaseLaunchForceMultiplier => _baseLaunchForceMultiplier;
        public float MinimumSpeedThreshold => _minimumSpeedThreshold;

        //This will be used if in the future the config can be overriden from some source
        public void LoadFromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public string ToJson(bool prettyPrint = false)
        {
            return JsonUtility.ToJson(this, prettyPrint);
        }
    }
}