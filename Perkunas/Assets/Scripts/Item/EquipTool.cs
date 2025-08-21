using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public bool isRangedWeapon;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    
    public int damage;

    private Animator animator;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if(!attacking)
        {
            if(CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        SoundManager.Instance.AttackClip();
        if (!isRangedWeapon)
        {
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, attackDistance))
            {
                if(doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
                {
                    resource.Gather(hit.point, hit.normal);
                }

                if (doesDealDamage && hit.collider.TryGetComponent(out IDamagable damagable))
                {
                    Debug.Log($"공격: ");
                    damagable.TakePhysicalDamage(damage);
                }
            }
        }
        else
        {
            Debug.Log("원거리 공격");
            GameObject go = Instantiate(projectilePrefab, CharacterManager.Instance.player.projectileSpawn.position, Quaternion.identity);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            Projectile proj = go.GetComponent<Projectile>();
            proj.Init(damage);
            if(rb != null) rb.AddForce(Camera.main.transform.forward * projectileSpeed, ForceMode.Impulse);
        }
    }

}
