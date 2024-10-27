using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 3)]
public class PlayersData : ScriptableObject
{
    public Dictionary<InputDevice, PlayerInfo> players = new Dictionary<InputDevice, PlayerInfo>();
}