using UnityEngine;

public class KnockbackOnCollision : MonoBehaviour
{
    [SerializeField]
    private float knockbackMultiplier = 1f; // Multiplicateur pour ajuster la force de propulsion
    [SerializeField]
    private float verticalLift = 1f; // Ajout vertical à la force (ajustable depuis l'inspecteur)

    private void OnCollisionEnter(Collision collision)
    {
        // Vérifie si l'objet qui percute a le tag "Player"
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("collision");
            // Récupère le Rigidbody des deux objets
            Rigidbody thisRb = GetComponent<Rigidbody>();
            Rigidbody playerRb = collision.collider.GetComponent<Rigidbody>();

            if (thisRb != null && playerRb != null)
            {
                // Calcule la différence de vitesse entre les deux objets
                Vector3 velocityDifference = playerRb.velocity - thisRb.velocity;

                // Calcule la direction de l'impact
                Vector3 direction = transform.position - collision.transform.position;
                direction.Normalize(); // Normalise pour obtenir une direction unitaire

                // Ajoute une composante verticale configurable à la direction
                direction.y += verticalLift;

                // Renormalise pour garantir une direction valide
                direction.Normalize();

                // Applique une force basée sur la magnitude de la différence de vitesses
                float force = velocityDifference.magnitude * knockbackMultiplier;
                thisRb.AddForce(direction * force, ForceMode.Impulse);
            }
        }
    }
}
