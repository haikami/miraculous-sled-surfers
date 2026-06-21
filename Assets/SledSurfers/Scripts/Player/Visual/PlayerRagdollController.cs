using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerRagdollController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody[] _ragdollBones; // all bone Rigidbodies, set in Inspector
        [SerializeField] private Collider _mainPlayerCollider;
        [SerializeField] private Rigidbody _mainPlayerRigidbody;

        private void Awake()
        {
            // SetRagdollActive(false); // start in animated, non-ragdoll state
        }

        public void Activate(Vector3 impactForce)
        {
            SetRagdollActive(true);

            foreach (var bone in _ragdollBones)
                bone.AddForce(impactForce, ForceMode.Impulse);
        }

        public void Deactivate()
        {
            SetRagdollActive(false);
        }

        private void SetRagdollActive(bool active)
        {
            _animator.enabled = !active;
            _mainPlayerCollider.enabled = !active;
            _mainPlayerRigidbody.isKinematic = active; // main body goes kinematic, ragdoll bones take over

            foreach (var bone in _ragdollBones)
            {
                bone.isKinematic = !active;
                bone.GetComponent<Collider>().enabled = active;
            }
        }
    }
}