using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    ///     Apply forward force to instantiated prefab
    /// </summary>
    public class LaunchProjectile : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The projectile that's created")]
        private GameObject m_ProjectilePrefab;

        [SerializeField]
        [Tooltip("The point that the project is created")]
        private Transform m_StartPoint;

        [SerializeField]
        [Tooltip("The speed at which the projectile is launched")]
        private float m_LaunchSpeed = 1.0f;

        public void Fire()
        {
            GameObject newObject = Instantiate(m_ProjectilePrefab, m_StartPoint.position, m_StartPoint.rotation, null);

            if (newObject.TryGetComponent(out Rigidbody rigidBody))
                ApplyForce(rigidBody);
        }

        private void ApplyForce(Rigidbody rigidBody)
        {
            Vector3 force = m_StartPoint.forward * m_LaunchSpeed;
            rigidBody.AddForce(force);
        }
    }
}