using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerInfo
{
    public int playerId;
    public int panelIndex;
    public int carIDSelected;
    public bool isRotating;
    public bool isValidateSelection = false;
    public PlayerInput _playerInput;
    public bool isAlive = true;
    public Rect rect;
    public int rank;
    public int playerLife = 3;
    public float bumpPourcentage = 0;
    public bool isCheckStats;
    public List<string> listKilledBy;
    public List<string> listYouKilled;


}