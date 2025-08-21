using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
        if (other.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("몬스터가 맞았다!");

            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakePhysicalDamage(damage);
            }
        }
    }
}
