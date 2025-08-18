using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [Header("Stats")] 
    public int health;
    public float walkSpeed;
    public float runSpeed;
    // TODO : need drop item 

    [Header("AI")] 
    private NavMeshAgent agent;
    public float detectDistance;
    
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
