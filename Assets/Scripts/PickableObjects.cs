using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PickableObject", order = 1)] 
public class PickableObjects : ScriptableObject
{
    public Sprite sprite;
    [TextArea(5,10)]
    public string note;
    public int fuel;
    public bool rocketBoots;
    public bool gun;
    public bool flashlight;
    [TextArea(5,10)]
    public string statsForUpgrades;
}
