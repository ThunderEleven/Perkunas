using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster")]
public class MonsterData : ScriptableObject
{
    public GameObject monsterPrefab;
    
    [Header("Stats")]
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public float detectDistance;
    
    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    
    [Header("Attacking")]
    public int damage;
    public float attackRate;
    public float attackDistance;
    public float fieldOfView;

    [Header("Range Monster Setting")]
    public bool isRangeMonster;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public Vector3 projectileRotation;

}
