using System;
using UnityEngine;

namespace SledSurfers.Scripts.Player
{
    public class PlayerMarker : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, new Vector3(2f, 1f, 2f));
        }
    }
}