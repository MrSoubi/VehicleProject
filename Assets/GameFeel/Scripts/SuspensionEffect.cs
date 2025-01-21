using UnityEngine;

public class SuspensionEffect : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Transform de l'objet qui contrôle réellement l'orientation/position du véhicule (ex: GameObject parent).")]
    public Transform carTransform;

    [Tooltip("Rigidbody du véhicule (celui qui a la physique).")]
    public Rigidbody carRigidbody;

    [Header("Réglages du tangage (pitch)")]
    [Tooltip("Facteur d'intensité de l'inclinaison avant/arrière.")]
    public float pitchFactor = 0.1f;

    [Header("Réglages de la gîte (roll)")]
    [Tooltip("Facteur d'intensité de l'inclinaison gauche/droite.")]
    public float rollFactor = 0.1f;

    [Header("Vitesse de lissage")]
    [Tooltip("Plus c’est grand, plus la transition est rapide (pour pitch et roll).")]
    public float smoothSpeed = 2f;

    // Variables internes pour stocker les angles courants
    private float currentPitch;
    private float currentRoll;

    // Vitesse du véhicule au frame précédent, pour calculer la variation
    private Vector3 lastVelocity;

    private void Start()
    {
        if (carRigidbody != null)
        {
            lastVelocity = carRigidbody.velocity;
        }
    }

    void Update()
    {
        if (carRigidbody == null || carTransform == null) return;

        // Récupère la vélocité actuelle
        Vector3 currentVelocity = carRigidbody.velocity;

        // Calcule la différence de vélocité (accélération brute)
        Vector3 velocityChange = currentVelocity - lastVelocity;

        // Divise par deltaTime pour obtenir une accélération sur la frame
        Vector3 worldAcceleration = velocityChange / Time.deltaTime;

        // Convertit cette accélération en repère local (par rapport à la voiture)
        Vector3 localAcceleration = carTransform.InverseTransformDirection(worldAcceleration);

        // --- Tangage (Pitch) ---
        // Si localAcceleration.z est positif, la voiture accélère vers l'avant,
        // on incline donc le nez vers le haut. On peut inverser si on veut l’effet inverse.
        float targetPitch = -localAcceleration.z * pitchFactor;
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, smoothSpeed * Time.deltaTime);

        // --- Gîte (Roll) ---
        // Si localAcceleration.x est positif, la voiture dérive sur la droite (tournant à droite),
        // elle doit donc s’incliner sur la gauche (rouler à gauche).
        // Ajustez le signe si vous souhaitez un comportement inverse.
        float targetRoll = localAcceleration.x * rollFactor;
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, smoothSpeed * Time.deltaTime);

        // Applique la rotation sur le mesh : X = pitch, Z = roll
        // Ici on ignore l'axe Y, mais vous pouvez le gérer si besoin
        transform.localRotation = Quaternion.Euler(currentPitch, 0f, currentRoll);

        // Stocke la vélocité pour la prochaine frame
        lastVelocity = currentVelocity;
    }
}
