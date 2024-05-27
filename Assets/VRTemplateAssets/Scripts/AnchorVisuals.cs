using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;

namespace Unity.VRTemplate
{
    /// <summary>
    ///     Helper script used to control the Teleport Anchor visuals animations.
    /// </summary>
    public class AnchorVisuals : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The animation for the vertical glow element on the platform.")]
        private Animation m_FadeAnimation;

        [SerializeField]
        [Tooltip("The arrow transform, at the center of the platform.")]
        private Transform m_Arrow;

        [SerializeField]
        [Tooltip("Height of the arrow transform when teleport ray hovers the teleport pad.")]
        private float m_TargetArrowHeight = 1.0f;

        [SerializeField]
        [Tooltip("Animation duration of the arrow transform to and from the target arrow height.")]
        private float m_ArrowAnimationDuration = 0.2f;

        [SerializeField]
        [Tooltip("Animation curve of hte arrow transform to and from the target arrow height.")]
        private AnimationCurve m_AnimationCurve;

        private Coroutine m_ArrowCoroutine;
        private Vector3TweenableVariable m_ArrowHeight;
        private Vector3 m_InitialArrowScale;

        private void Start()
        {
            if (m_FadeAnimation != null)
            {
                Animation fadeAnim = m_FadeAnimation;
                string clipName = m_FadeAnimation.clip.name;
                fadeAnim[clipName].normalizedTime = 1f;
            }

            m_ArrowHeight = new Vector3TweenableVariable();
            m_ArrowHeight.animationCurve = m_AnimationCurve;
            m_InitialArrowScale = m_Arrow.localScale;
        }

        private void Update()
        {
            m_Arrow.localPosition = m_ArrowHeight.Value;
        }

        /// <summary>
        ///     Performs animations when teleport interactor enters the teleport anchor selection.
        /// </summary>
        public void OnAnchorEnter()
        {
            m_Arrow.localScale = m_InitialArrowScale;

            if (m_FadeAnimation != null)
            {
                Animation fadeAnim = m_FadeAnimation;
                string clipName = m_FadeAnimation.clip.name;
                fadeAnim[clipName].normalizedTime = 0f;
                fadeAnim[clipName].speed = 1f;
                fadeAnim.Play();
            }

            if (m_ArrowCoroutine != null)
                StopCoroutine(m_ArrowCoroutine);

            Vector3 arrowPosition = m_Arrow.localPosition;
            m_ArrowCoroutine = StartCoroutine(m_ArrowHeight.PlaySequence(arrowPosition,
                new float3(arrowPosition.x, m_TargetArrowHeight, arrowPosition.z), m_ArrowAnimationDuration));
        }

        /// <summary>
        ///     Performs animations when teleport interactor exits the teleport anchor selection.
        /// </summary>
        public void OnAnchorExit()
        {
            if (m_FadeAnimation != null)
            {
                // Set time to 1, at the end of the animation, play at 1.5x speed
                Animation fadeAnim = m_FadeAnimation;
                string clipName = m_FadeAnimation.clip.name;
                fadeAnim[clipName].normalizedTime = 1f;
                fadeAnim[clipName].speed = -1.5f;
                fadeAnim.Play();
            }

            if (m_ArrowCoroutine != null)
                StopCoroutine(m_ArrowCoroutine);

            Vector3 arrowPosition = m_Arrow.localPosition;
            m_ArrowCoroutine = StartCoroutine(m_ArrowHeight.PlaySequence(arrowPosition,
                new float3(arrowPosition.x, 0, arrowPosition.z), m_ArrowAnimationDuration));
        }

        /// <summary>
        ///     Hides the arrow visual when teleporting
        /// </summary>
        public void HideArrowOnTeleport()
        {
            m_Arrow.localScale = Vector3.zero;
        }
    }
}