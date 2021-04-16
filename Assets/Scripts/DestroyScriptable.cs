using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Destroy", order = 4)]
public class DestructionUtil : ScriptableObject
{
    public void Destroy(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }
}
