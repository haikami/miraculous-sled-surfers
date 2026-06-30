using SledSurfers.Scripts.Core;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        
        private static readonly int IsAirborneParam = Animator.StringToHash("IsAirborne");
        private static readonly int IsPlayingParam = Animator.StringToHash("IsPlaying");

        public void SetPlayingState() => _animator.SetBool(IsPlayingParam, true);
        public void SetIdleState() => _animator.SetBool(IsPlayingParam, false);
        public void SetAirborne(bool isAirborne) => _animator.SetBool(IsAirborneParam, isAirborne);
    }
}