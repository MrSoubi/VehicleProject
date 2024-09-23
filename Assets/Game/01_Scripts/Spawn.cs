using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] int gamepadIndex;
    [SerializeField] float viewportX, viewportY, viewportHeight, viewportWidth;

    // Start is called before the first frame update
    void Start()
    {
        GameObject car = Instantiate(carPrefab, transform.position, transform.rotation);

        car.GetComponent<CarController>().setGamepadIndex(gamepadIndex);

        Rect viewport = new Rect(viewportX, viewportY, viewportWidth, viewportHeight);

        car.GetComponent<CarController>().GetCamera().rect = viewport;


    }
}
