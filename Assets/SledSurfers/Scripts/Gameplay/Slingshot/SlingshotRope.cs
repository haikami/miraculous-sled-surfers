using UnityEngine;

namespace SledSurfers.Scripts.Gameplay.Slingshot
{
    public class SlingshotRope : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Transform _leftAnchor;
        [SerializeField] private Transform _rightAnchor;
        [SerializeField] private Transform _playerVisual;
        
        private bool _isListening;

        private Vector3 IdlePosition =>
            Vector3.Lerp(_leftAnchor.position, _rightAnchor.position, 0.5f) + Vector3.down * 0.1f;

        private void Awake()
        {
            _lineRenderer.positionCount = 5;
            _lineRenderer.useWorldSpace = true;
            SetIdleMode();
        }
        
        public void StartListening()
        {
            _isListening = true;
        }

        public void StopListening()
        {
            _isListening = false;
            SetIdleMode();
        }

        private void LateUpdate()
        {
            if (!_isListening) return;
            Setup(_playerVisual.position);
        }
        
        private void SetIdleMode() => Setup(IdlePosition);
        
        private void Setup(Vector3 position)
        {
            _lineRenderer.SetPosition(0, _leftAnchor.position);
            _lineRenderer.SetPosition(1, Vector3.Lerp(_leftAnchor.position, position, 0.5f) + Vector3.down * 0.1f);
            _lineRenderer.SetPosition(2, position);
            _lineRenderer.SetPosition(3, Vector3.Lerp(position, _rightAnchor.position, 0.5f) + Vector3.down * 0.1f);
            _lineRenderer.SetPosition(4, _rightAnchor.position);
        }
    }
}