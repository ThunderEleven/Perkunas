using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{

    public int damage;

    public void Init(int damage)
    {
        this.damage = damage;
    }
    private void OnCollisionEnter(Collision other)
    { 
        if (other.collider.CompareTag("Player"))
        {
            Debug.Log("플레이어가 맞았다!");

            if (other.collider.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakePhysicalDamage(damage);
            }
            
        }
        else if (other.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
