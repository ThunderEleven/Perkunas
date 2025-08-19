using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTest : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if (Input.GetKeyDown(KeyCode.F) && other.collider.CompareTag("Monster"))
        {
            if (other.gameObject.TryGetComponent(out Monster monster))
            {
                monster.TakeDamage(5);
                Debug.Log("데미지를 입히다.");
            }
        }
    }
}
