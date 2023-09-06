using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/new unit data")]
public class UnitData : ScriptableObject
{
    public int health;
    public int damage;
    public float timeBtwAttacks;
    public float speed;
    public float stopDistance;
    [Range(0,1)]public float avoidAttack;
}
