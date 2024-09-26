using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public Dictionary<InputDevice, PlayerInfo> playerPanelMapping = new Dictionary<InputDevice, PlayerInfo>();
}