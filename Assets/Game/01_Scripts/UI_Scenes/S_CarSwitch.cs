using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class S_CarSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private RawImage[] carDisplays;
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed  = 30f;
    public S_CarSelection S_CarSelection;
    private Dictionary<InputDevice, PlayerInfo> players => S_CarSelection.ReturnPlayerInfo();



    private float totalRotation = 0f;

    private bool isRotating = false;
    private bool isRotatingRight = true;
    private int _currentPlayerID = 0;

    private void Update()
    {
       

        if (!isRotating) 
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isRotating = true;
                isRotatingRight = true;
                totalRotation = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isRotating = true;
                isRotatingRight = false;
                totalRotation = 0f;
            }
        }

        if (isRotating)
        {
            if (totalRotation < 90f)
            {
                RotateAroundTarget(_currentPlayerID);
            }
            else
            {
                isRotating = false;
            }
        }
    }

    private void RotateAroundTarget(int CameraNumber)
    {
       
        float rotationThisFrame = _rotationSpeed * Time.deltaTime;
       
        if (totalRotation + rotationThisFrame >= 90f)
        {
            rotationThisFrame = 90f - totalRotation;
            
        }
        Vector3 rotationDirection = isRotatingRight ? Vector3.down : Vector3.up;
        

        _cameras[CameraNumber].transform.RotateAround(_target.position, rotationDirection, rotationThisFrame);      
        totalRotation += rotationThisFrame;    
        
    }

    public void SwitchCar(InputAction.CallbackContext context)
    {
        int ID = 0;
        
        foreach (var value in players)
        {
            if (value.Key == context.control.device)
            {
                ID = value.Value.playerId;
                break;
            }
        }
        _currentPlayerID = ID;

        Debug.Log( $"Switch {ID}");
        if (context.performed)
        {
            
            float input = context.ReadValue<float>();
            if (input > 0) 
            {
                isRotating = true;
                isRotatingRight = true;
                totalRotation = 0f;

                players[context.control.device].carIDSelected += 1;

                if (players[context.control.device].carIDSelected == -1)
                {
                    players[context.control.device].carIDSelected = _cars.Length;
                }
                else if (players[context.control.device].carIDSelected == _cars.Length + 1)
                {
                    players[context.control.device].carIDSelected = 0;
                }
            }
            else if (input < 0) 
            {
                isRotating = true;
                isRotatingRight = false;
                totalRotation = 0f;

                players[context.control.device].carIDSelected -= 1;

                if (players[context.control.device].carIDSelected == -1)
                {
                    players[context.control.device].carIDSelected = _cars.Length;
                }
                else if (players[context.control.device].carIDSelected == _cars.Length + 1)
                {
                    players[context.control.device].carIDSelected = 0;
                }
            }
            Debug.Log("car ID selected: " + players[context.control.device].carIDSelected);
            
        }
    }

    public void OnValidateButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            
        }
    }

}
