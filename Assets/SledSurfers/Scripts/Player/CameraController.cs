using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 _idleOffset = new Vector3(0, 4, -8);
    public Vector3 _mainMenuOffset = new Vector3(0, 4, 8);
    public float smoothSpeed = 5f;

    private bool _following = false;

    public void ToMainMenuView(Transform playerPosition)
    {
        _following = false;
        transform.position = playerPosition.position + _mainMenuOffset;
        transform.LookAt(target);
    }

    public void ToIdleView()
    {
        _following = false;
        transform.position = target.position + _idleOffset;
        transform.LookAt(target);
    }
    
    
    void LateUpdate()
    {
        if (!_following) return;
        
        
        var desiredPosition = target.position + target.TransformDirection(_idleOffset);
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime);

        transform.LookAt(target);
    }
}