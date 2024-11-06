using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData", order = 2)]
public class MapsData : ScriptableObject
{
    public string MapNameScene;
    public GameObject MapObject;
    public string MapNameDisplay;

}
