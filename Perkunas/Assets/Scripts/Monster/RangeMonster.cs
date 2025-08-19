using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : Monster
{
    [Header("Range Weapon Settings")] 
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float projectileSpeed;
    public Vector3 projectileRotation;
    
    public override void Attack()
    {
        base.Attack();
        if (animator != null)
        {
            animator.speed = 1f;
            Debug.Log("원거리 공격");
            animator.SetTrigger("Attack");
            Vector3 playerDirection = (CharacterManager.Instance.Player.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.LookRotation(playerDirection));
            projectile.transform.Rotate(projectileRotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if(rb != null) rb.AddForce(playerDirection * projectileSpeed, ForceMode.Impulse);
        }
    }
}
