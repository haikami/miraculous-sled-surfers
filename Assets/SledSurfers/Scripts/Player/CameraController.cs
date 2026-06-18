using SledSurfers.Scripts.Player;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController _playerController;
    
    [Header("Settings")]
    [SerializeField] Vector3 _idleOffset = new Vector3(0, 4, -8);
    [SerializeField] Vector3 _mainMenuOffset = new Vector3(0, 4, 8);
    [SerializeField] float _smoothSpeed = 5f;
    
    private Transform Target => _playerController.transform;
    
    private bool _following = false;

    public void ToMainMenuView(Transform playerPosition)
    {
        _following = false;
        transform.position = playerPosition.position + _mainMenuOffset;
        transform.LookAt(Target);
    }

    public void ToIdleView()
    {
        _following = false;
        transform.position = Target.position + _idleOffset;
        transform.LookAt(Target);
    }

    public void ToFollowing()
    {
        _following = true;
    }
    
    void LateUpdate()
    {
        if (!_following) return;
        
        
        var desiredPosition = Target.position + Target.TransformDirection(_idleOffset);
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            _smoothSpeed * Time.deltaTime);

        transform.LookAt(Target);
    }
}