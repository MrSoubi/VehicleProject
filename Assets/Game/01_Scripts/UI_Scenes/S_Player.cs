using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_Player : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnDestroy()
    {
        //input.SwitchCurrentActionMap(null);

        input.DeactivateInput();

    }
    void Start()
    {
        input.SwitchCurrentActionMap("MenuSelection");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
