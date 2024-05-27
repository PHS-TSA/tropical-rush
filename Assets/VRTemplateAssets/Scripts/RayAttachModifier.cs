using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.VRTemplate
{
    /// <summary>
    ///     Add this to your interactable to make it snap to the source of the XR Ray Interactor
    ///     instead of staying at a distance. Has a similar outcome as enabling Force Grab.
    /// </summary>
    public class RayAttachModifier : MonoBehaviour
    {
        private IXRSelectInteractable m_SelectInteractable;

        protected void OnEnable()
        {
            m_SelectInteractable = GetComponent<IXRSelectInteractable>();
            if (m_SelectInteractable as Object == null)
            {
                Debug.LogError($"Ray Attach Modifier missing required Select Interactable on {name}", this);
                return;
            }

            m_SelectInteractable.selectEntered.AddListener(OnSelectEntered);
        }

        protected void OnDisable()
        {
            if (m_SelectInteractable as Object != null)
                m_SelectInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (!(args.interactorObject is XRRayInteractor))
                return;

            Transform attachTransform = args.interactorObject.GetAttachTransform(m_SelectInteractable);
            Pose originalAttachPose = args.interactorObject.GetLocalAttachPoseOnSelect(m_SelectInteractable);
            attachTransform.SetLocalPose(originalAttachPose);
        }
    }
}