using UnityEngine;

public class CarCameraController3Cases : MonoBehaviour
{
    [Header("Cible � suivre")]
    public Transform target;           // Transform de la voiture
    public Rigidbody carRigidbody;     // Pour conna�tre la v�locit�

    [Header("D�tection voiture en l�air")]
    public RSO_IsCarGrounded iscarGrounded;

    [Header("Positionnement cam�ra")]
    public float distance = 5f;        // Distance id�ale derri�re la voiture
    public float height = 2f;          // Hauteur de la cam�ra

    [Header("Recentrage auto (rotation)")]
    public float returnSpeed = 5f;     // Vitesse de recentrage automatique
    public float defaultPitch = 10f;   // Inclinaison verticale par d�faut quand on se recentre
    public float minPitch = -60f;
    public float maxPitch = 60f;

    [Header("Damping de suivi (position)")]
    public float followDamping = 5f;        // Vitesse de �rattrapage� de la position cible
    public float minCameraDistance = 2f;    // Distance min entre la cam�ra et la voiture
    public float maxCameraDistance = 10f;   // Distance max entre la cam�ra et la voiture

    // Variables internes pour la rotation
    private float yaw = 0f;   // Rotation horizontale (axe Y)
    private float pitch = 0f; // Rotation verticale (axe X)

    private void Start()
    {
        // Initialiser la cam�ra derri�re la voiture (optionnel)
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

        // La cam�ra se recentre automatiquement en permanence (aucun contr�le joystick)
        AutoCenterCamera();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // 1) Calcul de la rotation finale de la cam�ra
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // 2) Position id�ale : offset local [0, height, -distance]
        Vector3 desiredPosition = target.position + rotation * new Vector3(0f, height, -distance);

        // 3) D�termination de la direction de la v�locit�
        Vector3 velocity = carRigidbody.linearVelocity;
        float speed = velocity.magnitude;

        // Si la vitesse est trop faible, damping global
        if (speed < 0.01f)
        {
            Vector3 nextPos = Vector3.Lerp(transform.position, desiredPosition, followDamping * Time.deltaTime);
            ApplyDistanceClampAndSetPosition(nextPos);
        }
        else
        {
            // On applique un damping uniquement dans l�axe de la v�locit�
            Vector3 velocityDir = velocity.normalized;
            Vector3 currentPos = transform.position;
            Vector3 toDesired = desiredPosition - currentPos;

            // Projection parall�le et perpendiculaire
            Vector3 parallel = Vector3.Project(toDesired, velocityDir);
            Vector3 perpendicular = toDesired - parallel;

            // Lerp seulement sur la composante parall�le
            Vector3 parallelDamped = Vector3.Lerp(Vector3.zero, parallel, followDamping * Time.deltaTime);
            Vector3 nextPos = currentPos + perpendicular + parallelDamped;

            ApplyDistanceClampAndSetPosition(nextPos);
        }

        // Oriente la cam�ra vers la voiture (l�g�rement au-dessus, si d�sir�)
        Vector3 lookTarget = target.position + Vector3.up * height * 0.5f;
        transform.LookAt(lookTarget);
    }

    /// <summary>
    /// Recentrage automatique de la rotation (yaw/pitch),
    /// selon 2 cas : au sol (orientation de la voiture) ou en l�air (v�locit�).
    /// </summary>
    private void AutoCenterCamera()
    {
        float desiredYaw;
        float desiredPitch;

        if (!iscarGrounded.Value)
        {
            // CAS "en l�air" : align� sur la direction de la v�locit�
            Vector3 vel = carRigidbody.linearVelocity;
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
    /// Applique la limite min/max de distance et d�finit la position de la cam�ra.
    /// </summary>
    private void ApplyDistanceClampAndSetPosition(Vector3 candidatePos)
    {
        float distToTarget = Vector3.Distance(candidatePos, target.position);

        if (distToTarget < minCameraDistance)
        {
            // Trop pr�s : on pousse la cam�ra � minCameraDistance
            candidatePos = target.position +
                           (candidatePos - target.position).normalized * minCameraDistance;
        }
        else if (distToTarget > maxCameraDistance)
        {
            // Trop loin : on rapproche la cam�ra � maxCameraDistance
            candidatePos = target.position +
                           (candidatePos - target.position).normalized * maxCameraDistance;
        }

        transform.position = candidatePos;
    }
}
