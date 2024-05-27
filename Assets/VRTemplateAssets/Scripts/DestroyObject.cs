using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    ///     Destroys GameObject after a few seconds.
    /// </summary>
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Time before destroying in seconds.")]
        private float m_Lifetime = 5f;

        private void Start()
        {
            Destroy(gameObject, m_Lifetime);
        }
    }
}