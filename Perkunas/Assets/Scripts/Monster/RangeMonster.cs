using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    public Transform projectileSpawn;
    public override void Attack()
    {
        base.Attack();
        if (animator != null)
        {
            animator.speed = 1f;
            Debug.Log("원거리 공격");
            animator.SetTrigger("Attack");
            Vector3 playerDirection = (CharacterManager.Instance.Player.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(data.projectilePrefab, projectileSpawn.position, Quaternion.LookRotation(playerDirection));
            projectile.transform.Rotate(data.projectileRotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if(rb != null) rb.AddForce(playerDirection * data.projectileSpeed, ForceMode.Impulse);
        }
    }
}
