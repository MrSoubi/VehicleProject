using UnityEngine;

public class KnockbackOnCollision : MonoBehaviour
{
    [SerializeField]
    private float knockbackMultiplier = 1f; // Multiplicateur pour ajuster la force de propulsion
    [SerializeField]
    private float verticalLift = 1f; // Ajout vertical � la force (ajustable depuis l'inspecteur)

    private void OnCollisionEnter(Collision collision)
    {
        // V�rifie si l'objet qui percute a le tag "Player"
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("collision");
            // R�cup�re le Rigidbody des deux objets
            Rigidbody thisRb = GetComponent<Rigidbody>();
            Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();

            if (thisRb != null && playerRb != null)
            {
                // Calcule la diff�rence de vitesse entre les deux objets
                Vector3 velocityDifference = playerRb.linearVelocity - thisRb.linearVelocity;

                // Calcule la direction de l'impact
                Vector3 direction = transform.position - collision.transform.position;
                direction.Normalize(); // Normalise pour obtenir une direction unitaire

                // Ajoute une composante verticale configurable � la direction
                direction.y += verticalLift;

                // Renormalise pour garantir une direction valide
                direction.Normalize();

                // Applique une force bas�e sur la magnitude de la diff�rence de vitesses
                float force = velocityDifference.magnitude * knockbackMultiplier;
                thisRb.AddForce(direction * force, ForceMode.Impulse);
            }
        }
    }
}
