using UnityEngine;

public class SuspensionEffect : MonoBehaviour
{
    [Header("R�f�rences")]
    [Tooltip("Transform de l'objet qui contr�le r�ellement l'orientation/position du v�hicule (ex: GameObject parent).")]
    public Transform carTransform;

    [Tooltip("Rigidbody du v�hicule (celui qui a la physique).")]
    public Rigidbody carRigidbody;

    [Header("R�glages du tangage (pitch)")]
    [Tooltip("Facteur d'intensit� de l'inclinaison avant/arri�re.")]
    public float pitchFactor = 0.1f;

    [Header("R�glages de la g�te (roll)")]
    [Tooltip("Facteur d'intensit� de l'inclinaison gauche/droite.")]
    public float rollFactor = 0.1f;

    [Header("Vitesse de lissage")]
    [Tooltip("Plus c�est grand, plus la transition est rapide (pour pitch et roll).")]
    public float smoothSpeed = 2f;

    // Variables internes pour stocker les angles courants
    private float currentPitch;
    private float currentRoll;

    // Vitesse du v�hicule au frame pr�c�dent, pour calculer la variation
    private Vector3 lastVelocity;

    private void Start()
    {
        if (carRigidbody != null)
        {
            lastVelocity = carRigidbody.linearVelocity;
        }
    }

    void Update()
    {
        if (carRigidbody == null || carTransform == null) return;

        // R�cup�re la v�locit� actuelle
        Vector3 currentVelocity = carRigidbody.linearVelocity;

        // Calcule la diff�rence de v�locit� (acc�l�ration brute)
        Vector3 velocityChange = currentVelocity - lastVelocity;

        // Divise par deltaTime pour obtenir une acc�l�ration sur la frame
        Vector3 worldAcceleration = velocityChange / Time.deltaTime;

        // Convertit cette acc�l�ration en rep�re local (par rapport � la voiture)
        Vector3 localAcceleration = carTransform.InverseTransformDirection(worldAcceleration);

        // --- Tangage (Pitch) ---
        // Si localAcceleration.z est positif, la voiture acc�l�re vers l'avant,
        // on incline donc le nez vers le haut. On peut inverser si on veut l�effet inverse.
        float targetPitch = -localAcceleration.z * pitchFactor;
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, smoothSpeed * Time.deltaTime);

        // --- G�te (Roll) ---
        // Si localAcceleration.x est positif, la voiture d�rive sur la droite (tournant � droite),
        // elle doit donc s�incliner sur la gauche (rouler � gauche).
        // Ajustez le signe si vous souhaitez un comportement inverse.
        float targetRoll = localAcceleration.x * rollFactor;
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, smoothSpeed * Time.deltaTime);

        // Applique la rotation sur le mesh : X = pitch, Z = roll
        // Ici on ignore l'axe Y, mais vous pouvez le g�rer si besoin
        transform.localRotation = Quaternion.Euler(currentPitch, 0f, currentRoll);

        // Stocke la v�locit� pour la prochaine frame
        lastVelocity = currentVelocity;
    }
}
