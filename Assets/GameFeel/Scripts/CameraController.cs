using UnityEngine;

public class CarCameraController3Cases : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform target;           // Transform de la voiture
    public Rigidbody carRigidbody;     // Pour connaître la vélocité

    [Header("Détection voiture en l’air")]
    public RSO_IsCarGrounded iscarGrounded;

    [Header("Positionnement caméra")]
    public float distance = 5f;        // Distance idéale derrière la voiture
    public float height = 2f;          // Hauteur de la caméra

    [Header("Recentrage auto (rotation)")]
    public float returnSpeed = 5f;     // Vitesse de recentrage automatique
    public float defaultPitch = 10f;   // Inclinaison verticale par défaut quand on se recentre
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("Damping de suivi (position)")]
    public float followDamping = 5f;        // Vitesse de “rattrapage” de la position cible
    public float minCameraDistance = 2f;    // Distance min entre la caméra et la voiture
    public float maxCameraDistance = 10f;   // Distance max entre la caméra et la voiture

    // Variables internes pour la rotation
    private float yaw = 0f;   // Rotation horizontale (axe Y)
    private float pitch = 0f; // Rotation verticale (axe X)

    private void Start()
    {
        // Initialiser la caméra derrière la voiture (optionnel)
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            pitch = defaultPitch;
        }
    }

    private void Update()
    {
        if (target == null || carRigidbody == null)
            return;

        // La caméra se recentre automatiquement en permanence (aucun contrôle joystick)
        AutoCenterCamera();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // 1) Calcul de la rotation finale de la caméra
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // 2) Position idéale : offset local [0, height, -distance]
        Vector3 desiredPosition = target.position + rotation * new Vector3(0f, height, -distance);

        // 3) Détermination de la direction de la vélocité
        Vector3 velocity = carRigidbody.velocity;
        float speed = velocity.magnitude;

        // Si la vitesse est trop faible, damping global
        if (speed < 0.01f)
        {
            Vector3 nextPos = Vector3.Lerp(transform.position, desiredPosition, followDamping * Time.deltaTime);
            ApplyDistanceClampAndSetPosition(nextPos);
        }
        else
        {
            // On applique un damping uniquement dans l’axe de la vélocité
            Vector3 velocityDir = velocity.normalized;
            Vector3 currentPos = transform.position;
            Vector3 toDesired = desiredPosition - currentPos;

            // Projection parallèle et perpendiculaire
            Vector3 parallel = Vector3.Project(toDesired, velocityDir);
            Vector3 perpendicular = toDesired - parallel;

            // Lerp seulement sur la composante parallèle
            Vector3 parallelDamped = Vector3.Lerp(Vector3.zero, parallel, followDamping * Time.deltaTime);
            Vector3 nextPos = currentPos + perpendicular + parallelDamped;

            ApplyDistanceClampAndSetPosition(nextPos);
        }

        // Oriente la caméra vers la voiture (légèrement au-dessus, si désiré)
        Vector3 lookTarget = target.position + Vector3.up * height * 0.5f;
        transform.LookAt(lookTarget);
    }

    /// <summary>
    /// Recentrage automatique de la rotation (yaw/pitch),
    /// selon 2 cas : au sol (orientation de la voiture) ou en l’air (vélocité).
    /// </summary>
    private void AutoCenterCamera()
    {
        float desiredYaw;
        float desiredPitch;

        if (!iscarGrounded.Value)
        {
            // CAS "en l’air" : aligné sur la direction de la vélocité
            Vector3 vel = carRigidbody.velocity;
            float velAngle = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
            desiredYaw = velAngle;
            desiredPitch = pitch;  // on ne change pas le pitch
        }
        else
        {
            // CAS "au sol" : on prend l'orientation de la voiture
            desiredYaw = target.eulerAngles.y;
            desiredPitch = defaultPitch;
        }

        // Transition douce des angles
        yaw = Mathf.LerpAngle(yaw, desiredYaw, Time.deltaTime * returnSpeed);
        pitch = Mathf.Lerp(pitch, desiredPitch, Time.deltaTime * returnSpeed);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    /// <summary>
    /// Applique la limite min/max de distance et définit la position de la caméra.
    /// </summary>
    private void ApplyDistanceClampAndSetPosition(Vector3 candidatePos)
    {
        float distToTarget = Vector3.Distance(candidatePos, target.position);

        if (distToTarget < minCameraDistance)
        {
            // Trop près : on pousse la caméra à minCameraDistance
            candidatePos = target.position +
                           (candidatePos - target.position).normalized * minCameraDistance;
        }
        else if (distToTarget > maxCameraDistance)
        {
            // Trop loin : on rapproche la caméra à maxCameraDistance
            candidatePos = target.position +
                           (candidatePos - target.position).normalized * maxCameraDistance;
        }

        transform.position = candidatePos;
    }
}
