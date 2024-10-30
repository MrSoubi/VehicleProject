using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColorManager : MonoBehaviour
{
    [SerializeField] Material yellowMaterial, greenMaterial, blueMaterial, redMaterial;
    [SerializeField] List<Renderer> renderers;

    public void SetColor(int playerID)
    {
        Material material;

        switch (playerID)
        {
            case 0:
                material = redMaterial;
                break;
            case 1:
                material = blueMaterial;
                break;
            case 2:
                material = greenMaterial;
                break;
            case 3:
                material = yellowMaterial;
                break;
            default:
                material = redMaterial;
                break;
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }
}
