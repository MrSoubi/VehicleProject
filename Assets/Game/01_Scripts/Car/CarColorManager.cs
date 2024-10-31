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
                material = greenMaterial;
                break;
            case 1:
                material = redMaterial;
                break;
            case 2:
                material = yellowMaterial;
                break;
            case 3:
                material = blueMaterial;
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
