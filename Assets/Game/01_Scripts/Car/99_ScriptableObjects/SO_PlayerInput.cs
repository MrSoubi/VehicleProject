using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Data/InputMap", order = 100)]
public class SO_PlayerInput : ScriptableObject
{
    public string throttle;
    public string reverse;
    public string steer;
    public string pitch;
    public string jump;
    public string boost;
}
