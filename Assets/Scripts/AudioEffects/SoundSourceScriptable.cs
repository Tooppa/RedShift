using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundSourceScriptable", order = 3)]
public class SoundSourceScriptable : ScriptableObject
{
    public Vector2 location;
    public AudioSource audio;
    public bool loop;

}
