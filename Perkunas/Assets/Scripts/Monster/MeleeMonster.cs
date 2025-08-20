using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : Monster
{
    public override void Attack()
    {
        base.Attack();
        if (animator != null)
        {
            animator.speed = 1f;
            Debug.Log("근거리 공격");
            animator.SetTrigger("Attack");
            CharacterManager.Instance.Player.condition.TakePhysicalDamage(data.damage);
        }
    }
}
