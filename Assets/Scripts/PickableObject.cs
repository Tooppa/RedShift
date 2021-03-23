using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PickableObject", order = 1)] 
public class PickableObject : ScriptableObject
{
    public new string name;

    public Sprite sprite;
}