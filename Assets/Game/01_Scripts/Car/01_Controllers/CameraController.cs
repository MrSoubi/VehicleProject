using UnityEngine;
using UnityEngine.InputSystem; // Pour accéder à Gamepad

public class CameraController : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform target;           // Transform de la voiture
    public Rigidbody carRigidbody;     // Pour connaître la vélocité

    public RSO_IsCarGrounded isCarGrounded;

    [Header("Positionnement caméra")]
    public float distance = 5f;        // Distance derrière la voiture
    public float height = 2f;          // Hauteur de la caméra

    [Header("Contrôle manuel")]
    public float rotationSpeed = 100f; // Sensibilité du stick droit

    [Header("Recentrage auto")]
    public float returnSpeed = 5f;     // Vitesse de recentrage automatique
    public float defaultPitch = 10f;   // Inclinaison verticale par défaut quand on se recentre

    [Header("Limites verticales")]
    public float minPitch = -60f;
    public float maxPitch = 60f;

    // Variables internes pour la rotation de la caméra
    private float yaw = 0f;
    private float pitch = 0f;

    private void Start()
    {
        // Initialiser la caméra directement derrière la voiture (optionnel)
        if (target != null)
        {
            yaw = target.eulerAngles.y + 180f;
            pitch = defaultPitch;
        }
    }

    private void Update()
    {
        // Sécurité : on arrête si pas de cible ou pas de manette
        if (target == null || carRigidbody == null || Gamepad.all.Count == 0)
            return;

        Gamepad gamepad = Gamepad.all[0];
        if (gamepad == null) return;

        // Récupère l’input du stick droit
        Vector2 rightStick = gamepad.rightStick.ReadValue();
        float stickMagnitude = rightStick.magnitude;

        // --- 1) Contrôle manuel si l’utilisateur bouge le stick droit ---
        if (stickMagnitude > 0.01f)
        {
            yaw += rightStick.x * rotationSpeed * Time.deltaTime;
            pitch -= rightStick.y * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else
        {
            // --- 2) Recentrage automatique si le stick est relâché ---
            AutoCenterCamera();
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Calcule la rotation en Quaternion
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Calcule la position désirée de la caméra
        Vector3 desiredPosition = target.position + rotation * new Vector3(0f, height, -distance);

        // Applique la position
        transform.position = desiredPosition;

        // Oriente la caméra vers la voiture (avec un léger offset vertical si souhaité)
        Vector3 lookTarget = target.position + Vector3.up * height * 0.5f;
        transform.LookAt(lookTarget);
    }

    /// <summary>
    /// Gère le recentrage automatique de la caméra
    /// en fonction de la situation (avant, arrière, ou en l’air).
    /// </summary>
    private void AutoCenterCamera()
    {
        float desiredYaw;
        float desiredPitch;

        if (!isCarGrounded.Value)
        {
            // --- CAS 3 : la voiture est en l’air ---
            // On s’aligne sur la direction de la vélocité
            Vector3 velocity = carRigidbody.velocity;
            // Angle horizontal à partir de la vélocité
            float velocityAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            desiredYaw = velocityAngle;
            desiredPitch = defaultPitch;           // On peut choisir un pitch par défaut
        }
        else
        {
            // --- CAS 1 et 2 : la voiture est au sol ---
            // On différencie la marche avant et arrière via la vélocité locale
            Vector3 localVelocity = target.InverseTransformDirection(carRigidbody.velocity);
            float zVel = localVelocity.z;

            if (zVel >= 0)
            {
                // --- CAS 1 : Avancer ---
                // (Comportement identique à avant, mais séparé pour modifications futures)
                desiredYaw = target.eulerAngles.y;
                desiredPitch = defaultPitch;
            }
            else
            {
                // --- CAS 2 : Reculer ---
                // (Pour l’instant, on applique le même positionnement ; 
                //  on pourra le changer facilement si besoin)
                desiredYaw = target.eulerAngles.y + 180f;
                desiredPitch = defaultPitch;
            }
        }

        // On approche progressivement les angles "yaw" et "pitch" vers desiredYaw / desiredPitch
        yaw = Mathf.LerpAngle(yaw, desiredYaw, Time.deltaTime * returnSpeed);
        pitch = Mathf.Lerp(pitch, desiredPitch, Time.deltaTime * returnSpeed);
    }
}
