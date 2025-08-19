using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Returning
}

[Serializable]
public class Monster : MonoBehaviour
{
    // Data Setting
    public MonsterData data;
    [HideInInspector] public MonsterManager manager;
    [HideInInspector] public Vector3 groupKey;
    [HideInInspector] public Action<Vector3> onDeath; 
    
    // AI Setting
    protected NavMeshAgent agent;
    protected AIState aiState;
    protected bool isReturning = false;
    protected Vector3 spawnPosition;
    protected float spawnDistance;
    protected const float MaxDistFromSpawn = 10f;

    // Attack Setting
    protected float lastAttackTime;
    protected float playerDistance;

    
    // Local Variables 
    protected Animator animator;
    protected SkinnedMeshRenderer[] meshRenderers;
    protected float curHealth;

    public void Init(MonsterManager manager, Vector3 groupKey)
    {   
        this.manager = manager; 
        this.groupKey = groupKey;
    }
    
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        if(data == null) Debug.LogError("There's no monster data");
        spawnPosition = transform.position;
    }

    void Start()
    {
        SetState(AIState.Wandering);
        curHealth = data.maxHealth;
    }


    void Update()
    {
        // 플레이어와 몬스터간의 거리 계산
        playerDistance = Vector3.Distance(CharacterManager.Instance.Player.transform.position, transform.position);
        spawnDistance = Vector3.Distance(spawnPosition, transform.position);
        // 스폰 위치와 너무 떨어져있으면 돌아가도록 함
        if (!isReturning && spawnDistance > MaxDistFromSpawn)
        {
            SetState(AIState.Returning);
            isReturning = true;
        }
        
        // State가 멈춤 상태가 아니면 Moving 애니메이션 재생
        
        if(animator != null)  animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Returning:
                // Debug.Log("너무 멀어짐. 돌아감.");
                ReturningUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
        }
    }

    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = data.walkSpeed;
                agent.isStopped = true; // agent 멈추기
                break;
            case AIState.Wandering:
                agent.speed = data.walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = data.runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Returning:
                agent.speed = data.walkSpeed;
                agent.isStopped = false;
                break;
        }
    
        
        // Debug.Log($"Current State :  {aiState}");
        if(animator != null) animator.speed = agent.speed / data.walkSpeed;
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle); // 대기 상태로 전환.
            // 현재 상태가 방황함 + NavMeshAgent가 목표 지점까지 남은 거리가 거의 없을때
            // 랜덤 시간 뒤에 새 위치 찾는 함수 호출
            Invoke("WanderToNewLocation", Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
        }

        if (playerDistance < data.detectDistance)
        {
            SetState(AIState.Attacking);
            AttackingUpdate();
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;
        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        int i = 0;

        // 너무 가까우면 경로 탐색 다시
        // 최대 30번 가능
        do
        {
            NavMesh.SamplePosition(
            transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)),
            out hit,
            data.maxWanderDistance,
            NavMesh.AllAreas);
            i++;
        } while (Vector3.Distance(transform.position, hit.position) > data.detectDistance || i <= 30);

        return hit.position;
    }

    void ReturningUpdate()
    {
        // 스폰 위치와 가까워지면 다시 wandering으로 변경
        if (isReturning && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Wandering);
            isReturning = false;
        }
        else if (isReturning && spawnDistance > MaxDistFromSpawn)
        {
            // 고개 돌리기 전에 멈춤
            agent.isStopped = true;

            // 스폰 위치 바라보기
            Vector3 lookDirection = (spawnPosition - transform.position).normalized;
            lookDirection.y = 0; 
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            
            // 고개 돌리고 나서 이제 움직이도록 설정
            agent.isStopped = false;
            agent.SetDestination(spawnPosition);
        }
    }

    void AttackingUpdate()
    {
        // 플레이어와의 거리가 공격 범위 내에 있고, 시야각 내부에 있을 때
        if (playerDistance < data.attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;

            // 플레이어 방향 바라보기
            Vector3 lookDirection = (CharacterManager.Instance.Player.transform.position - transform.position).normalized;
            lookDirection.y = 0; 
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            if (Time.time - lastAttackTime > data.attackRate)
            {
                lastAttackTime = Time.time;
                Attack();
            }
        }
        else // 아닐 때
        {
            // 그럼에도 플레이어가 공격 범위 내에 있을 때
            if (playerDistance < data.detectDistance)
            {
                // 플레이어에게 가는 새로운 길을 또 만들어서 가려는 시도를 함
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                // 플레이어한테 갈 수 있으면 감
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else // 갈 수 없으면 추적을 멈추고 다시 Wandering 상태로 바꿈
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = false;
                    SetState(AIState.Wandering);
                }
            }
            else // 플레이어가 공격 범위 내에 있지 않을 때 추적을 멈추고 Wandering 상태로 바꿈
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        // 시야각 내로 들어왔는지 여부

        // 1. 몬스터가 플레이어를 바라보는 방향의 벡터를 만듬 : 플레이어 위치 - 몬스터 위치
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        // 2. 몬스터 위치와 몬스터->플레이어 방향 벡터 간의 각도를 구함
        float angle = Vector3.Angle(transform.position, directionToPlayer);
        // 3. 그 각도가 몬스터의 시야각보다 작으면 true, 아니면 false
        
        return angle < data.fieldOfView * 0.5f;
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            // 죽어야됨
            Die();
        }

        // 데미지 효과 
        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        // 몬스터 죽었을 때 아이템 떨구는 코드
        /*
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }
        */
        onDeath?.Invoke(groupKey);
        gameObject.SetActive(false);
    }

    public virtual void Attack()
    {  }

    
    IEnumerator DamageFlash()
    {
    // 데미지 효과 코드
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }
    
}

