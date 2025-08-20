using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    { 
        if (other.collider.CompareTag("Player"))
        {
            Debug.Log("플레이어가 맞았다!");
            // TODO : 플레이어 맞은거 처리
        }
        else if (other.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
