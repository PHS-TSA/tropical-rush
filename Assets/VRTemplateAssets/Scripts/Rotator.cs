using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    ///     Rotates this object at a user defined speed
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Angular velocity in degrees per second")]
        private Vector3 m_Velocity;

        private void Update()
        {
            transform.Rotate(m_Velocity * Time.deltaTime);
        }
    }
}