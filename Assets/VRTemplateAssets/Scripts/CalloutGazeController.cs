using UnityEngine;
using UnityEngine.Events;

namespace Unity.VRTemplate
{
    /// <summary>
    ///     Fires events when this object is is within the field of view of the gaze transform. This is currently used to
    ///     hide and show tooltip callouts on the controllers when the controllers are within the field of view.
    /// </summary>
    public class CalloutGazeController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The transform which the forward direction will be used to evaluate as the gaze direction.")]
        private Transform m_GazeTransform;

        [SerializeField]
        [Tooltip(
            "Threshold for the dot product when determining if the Gaze Transform is facing this object. The lower the threshold, the wider the field of view.")]
        [Range(0.0f, 1.0f)]
        private float m_FacingThreshold = 0.85f;

        [SerializeField]
        [Tooltip("Events fired when the Gaze Transform begins facing this game object")]
        private UnityEvent m_FacingEntered;

        [SerializeField]
        [Tooltip("Events fired when the Gaze Transform stops facing this game object")]
        private UnityEvent m_FacingExited;

        [SerializeField]
        [Tooltip(
            "Distance threshold for movement in a single frame that determines a large movement that will trigger Facing Exited events.")]
        private float m_LargeMovementDistanceThreshold = 0.05f;

        [SerializeField]
        [Tooltip("Cool down time after a large movement for Facing Entered events to fire again.")]
        private float m_LargeMovementCoolDownTime = 0.25f;

        private bool m_IsFacing;
        private float m_LargeMovementCoolDown;
        private Vector3 m_LastPosition;

        private void Update()
        {
            if (!m_GazeTransform)
                return;

            CheckLargeMovement();

            if (m_LargeMovementCoolDown < m_LargeMovementCoolDownTime)
                return;

            float dotProduct = Vector3.Dot(m_GazeTransform.forward,
                (transform.position - m_GazeTransform.position).normalized);
            if (dotProduct > m_FacingThreshold && !m_IsFacing)
                FacingEntered();
            else if (dotProduct < m_FacingThreshold && m_IsFacing)
                FacingExited();
        }

        private void CheckLargeMovement()
        {
            // Check if there is large movement
            Vector3 currentPosition = transform.position;
            float positionDelta = Mathf.Abs(Vector3.Distance(m_LastPosition, currentPosition));
            if (positionDelta > m_LargeMovementDistanceThreshold)
            {
                m_LargeMovementCoolDown = 0.0f;
                FacingExited();
            }

            m_LargeMovementCoolDown += Time.deltaTime;
            m_LastPosition = currentPosition;
        }

        private void FacingEntered()
        {
            m_IsFacing = true;
            m_FacingEntered.Invoke();
        }

        private void FacingExited()
        {
            m_IsFacing = false;
            m_FacingExited.Invoke();
        }
    }
}