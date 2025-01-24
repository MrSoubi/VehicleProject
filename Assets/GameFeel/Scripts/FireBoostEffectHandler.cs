using UnityEngine;

public class FireBoostEffectHandler : MonoBehaviour
{
    // Événement qui déclenche le boost
    [SerializeField] private RSE_BoostActivated boostActivated;

    // Référence à l'Animator
    [SerializeField] private Animator animator;

    // Nom du trigger pour l'animation
    [SerializeField] private string animationTriggerName = "BoostActivated";

    private void OnEnable()
    {
        boostActivated.trigger += HandleBoostActivation;
    }

    private void OnDisable()
    {
        boostActivated.trigger -= HandleBoostActivation;
    }

    private void HandleBoostActivation()
    {
        // Lancer l'animation via l'Animator
        if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
        {
            animator.SetTrigger(animationTriggerName);
        }
    }
}
