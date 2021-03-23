using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyScriptable", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public float speed;
    public float jumpHeight;
    public float enemyRange;
    public float attack;
    public float health;
}
