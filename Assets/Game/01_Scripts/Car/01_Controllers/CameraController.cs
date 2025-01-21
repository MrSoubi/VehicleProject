using UnityEngine;
using UnityEngine.InputSystem; // Pour accéder à Gamepad

public class CarCameraControllerDirectInput : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform target;         // Transform de la voiture
    public Rigidbody carRigidbody;   // Rigidbody pour connaître la vélocité

    [Header("Positionnement caméra")]
    public float distance = 5.0f;    // Distance derrière la voiture
    public float height = 2.0f;      // Hauteur de la caméra

    [Header("Contrôle manuel")]
    public float rotationSpeed = 100f;   // Sensibilité du stick droit

    [Header("Recentrage auto")]
    public float returnSpeed = 5f;       // Vitesse à laquelle la caméra revient derrière la voiture
    public float defaultPitch = 10f;     // Inclinaison verticale par défaut (en degrés), quand on se recentre

    [Header("Limites verticales")]
    public float minPitch = -60f;
    public float maxPitch = 60f;

    private float yaw = 0f;   // Rotation horizontale (autour de l’axe Y du monde)
    private float pitch = 0f; // Rotation verticale (autour de l’axe X du monde)

    void Start()
    {
        // Optionnel : initialiser la caméra directement derrière la voiture
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            pitch = defaultPitch;
        }
    }

    void Update()
    {
        // Sécurité : s’il n’y a pas de gamepad branché ou pas de cible, on ne fait rien
        if (Gamepad.all.Count == 0 || target == null || carRigidbody == null)
            return;

        Gamepad gamepad = Gamepad.all[0];
        if (gamepad == null) return;

        // Lecture du stick droit (valeur brute, sans PlayerInput)
        Vector2 rightStick = gamepad.rightStick.ReadValue();

        // -- 1) Mise à jour manuelle si l’utilisateur bouge le stick --
        float stickMagnitude = rightStick.magnitude;
        if (stickMagnitude > 0.01f)
        {
            // L’utilisateur est en train de bouger la caméra
            yaw += rightStick.x * rotationSpeed * Time.deltaTime;
            pitch -= rightStick.y * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else
        {
            // -- 2) Recentrage automatique si le stick est relâché --
            // Calcule la direction "avant" de la voiture, basée sur la vitesse
            Vector3 velocity = carRigidbody.velocity;
            Vector3 flatVelocity = new Vector3(velocity.x, 0f, velocity.z);

            float desiredYaw;
            if (flatVelocity.sqrMagnitude > 0.1f)
            {
                // Si la voiture se déplace, on se base sur la direction de la vitesse
                // Angle entre Vector3.forward (Z+) et la direction de la vélocité, +180 pour se mettre "derrière"
                float forwardAngle = Mathf.Atan2(flatVelocity.x, flatVelocity.z) * Mathf.Rad2Deg;
                desiredYaw = forwardAngle;
            }
            else
            {
                // Si la voiture est presque à l’arrêt, on se base sur son orientation
                desiredYaw = target.eulerAngles.y;
            }

            // On fait un "Lerp" (ou "LerpAngle") pour ramener progressivement la caméra derrière la voiture
            yaw = Mathf.LerpAngle(yaw, desiredYaw, Time.deltaTime * returnSpeed);
            pitch = Mathf.Lerp(pitch, defaultPitch, Time.deltaTime * returnSpeed);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calcul de la rotation finale de la caméra
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Position désirée : on applique la rotation à un offset [0, height, -distance]
        Vector3 desiredPosition = target.position + rotation * new Vector3(0f, height, -distance);

        // On place la caméra
        transform.position = desiredPosition;

        // On regarde la voiture (avec un léger offset en hauteur si voulu)
        Vector3 lookTarget = target.position + Vector3.up * height * 0.5f;
        transform.LookAt(lookTarget);
    }
}
