using System.Collections.Generic;
using UnityEngine;

public class LSDColorChanger : MonoBehaviour
{
    [Header("Durée de vie de l'objet (en secondes)")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Couleurs de départ et de fin")]
    [SerializeField] private Color startColor = Color.red;
    [SerializeField] private Color endColor = Color.blue;

    // Liste de tous les matériaux à changer
    private List<Material> materials = new List<Material>();

    // Chronomètre interne
    private float timer = 0f;

    void Start()
    {
        // Récupère tous les Renderers (y compris dans les enfants)
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // Pour chaque Renderer, on ajoute ses matériaux à la liste
        foreach (Renderer rend in renderers)
        {
            // "rend.materials" retourne un tableau de Material
            foreach (Material mat in rend.materials)
            {
                materials.Add(mat);
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // On calcule un ratio entre 0 et 1
        // 0 = début (startColor), 1 = fin (endColor)
        float t = timer / lifeTime;

        // On limite ce ratio à 1 pour éviter de dépasser
        t = Mathf.Clamp01(t);

        // On détermine la couleur actuelle par interpolation
        Color currentColor = Color.Lerp(startColor, endColor, t);

        // On applique la couleur interpolée à tous les matériaux
        foreach (Material mat in materials)
        {
            mat.color = currentColor;
        }

        // Quand le timer dépasse la durée de vie, on détruit l'objet
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
