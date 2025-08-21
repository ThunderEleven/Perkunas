using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IDamagable
{
    [SerializeField]float health = 21f;
    GameObject daddy;

    void Start()
    {
        daddy = transform.parent.gameObject;
    }
    void IDamagable.TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            Destroy(daddy);
        }
    }
}
