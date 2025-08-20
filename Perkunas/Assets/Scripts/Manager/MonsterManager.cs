using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterManager : MonoSingleton<MonsterManager>
{
    [Header("Monster Group Prefabs")]
    [SerializeField] private List<GameObject> monsterGroupPrefabs; // 여러 개의 프리팹 중 랜덤 선택

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 cornerA; // 평지의 꼭짓점 A
    [SerializeField] private Vector3 cornerB; // 평지의 꼭짓점 B
    [SerializeField] private int groupCount = 5; // 몬스터 그룹 갯수
    [SerializeField] private float minDistance = 5f; // 그룹 간 최소 간격
    [SerializeField] private int maxRetryCount = 30; // NavMesh 유효 위치 찾기 재시도 횟수
    [SerializeField] private float respawnDelay = 60f;
    
    private Dictionary<Vector3, GameObject> activeMonsterGroups = new Dictionary<Vector3, GameObject>();

    private Dictionary<Vector3, ObjectPool<GameObject>> groupPools = new Dictionary<Vector3, ObjectPool<GameObject>>();
    private List<Vector3> spawnPositions = new List<Vector3>();

    private void Awake()
    {
        Debug.Log("Monster Manager Awake");
        spawnPositions = GenerateRandomPositions(cornerA, cornerB, groupCount, minDistance);
        if(spawnPositions == null || spawnPositions.Count == 0) 
            Debug.LogError("Spawn Position is null");
        // 포지션 하나마다 몬스터 그룹 설정
        foreach (var pos in spawnPositions)
        {
            GameObject prefab = monsterGroupPrefabs[Random.Range(0, monsterGroupPrefabs.Count)];

            var pool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    GameObject obj = Instantiate(prefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                    obj.SetActive(false);
                    // 각 몬스터에 manager/그룹 키 알려주기
                    foreach (var m in obj.GetComponentsInChildren<Monster>())
                    {
                        m.Init(this, pos);
                        m.onDeath += OnMonsterDeath;
                    }
                    return obj;
                },
                actionOnRelease: (obj) => { obj.SetActive(false); },
                actionOnDestroy: (obj) => { Destroy(obj); },
                collectionCheck: true,
                defaultCapacity: 1, // 어차피 풀 관리 할 게 하나밖에 없음. 그룹단위라서
                maxSize: 5
            );

            groupPools[pos] = pool;
        }

        // 초기 스폰
        foreach (var pos in spawnPositions)
        {
            Debug.Log($"{pos}에서 생성됨");
            SpawnGroupAt(pos);
        }
    }

    /// <summary>
    /// 사각형의 네 꼭짓점 중 두 꼭짓점(a, b)를 받아서 count 만큼의 Vector3을 생성함. 이 때 각 Vector3은 minDist 이상의 거리 차이를 가지게 함.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="count"></param>
    /// <param name="minDist"></param>
    /// <returns></returns>
    private List<Vector3> GenerateRandomPositions(Vector3 a, Vector3 b, int count, float minDist)
    {
        List<Vector3> positions = new List<Vector3>();
        float minX = Mathf.Min(a.x, b.x);
        float maxX = Mathf.Max(a.x, b.x);
        float minZ = Mathf.Min(a.z, b.z);
        float maxZ = Mathf.Max(a.z, b.z);

        int safetyLimit = 500;
        int placed = 0;

        while (placed < count && safetyLimit > 0)
        {
            safetyLimit--;
            Vector3 randomPos = new Vector3(
                Random.Range(minX, maxX),
                a.y,
                Random.Range(minZ, maxZ)
            );

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                bool valid = true;
                foreach (var p in positions)
                {
                    if (Vector3.Distance(p, hit.position) < minDist)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    positions.Add(hit.position);
                    placed++;
                }
            }
            else
            {
                Debug.LogError("Cannot Make Spawn Position");
            }
        }

        if (positions == null || positions.Count == 0)
        {
            Debug.LogError("Cannot Make Spawn Positions");
        }
        return positions;
    }

    public void SpawnGroupAt(Vector3 pos)
    {
        if (!groupPools.ContainsKey(pos)) return;
        // Get 할때 GameObject SetActive True 해놔서 Get만 해도 됨
        var group = groupPools[pos].Get();
        group.SetActive(true);
        if(!activeMonsterGroups.ContainsKey(pos))
            activeMonsterGroups.Add(pos, group);
        
        // group이 active 되어도 안에 있는 애들이 inactive라 다시 세팅 해줘야함
        foreach (var m in group.GetComponentsInChildren<Monster>(true))
        {
            m.gameObject.SetActive(true);
        }
    }

    public void OnMonsterDeath(Vector3 groupKey)
    {
        if (!groupPools.ContainsKey(groupKey)) return;
        
        GameObject group = activeMonsterGroups[groupKey];
        bool allDead = false;
        
        // 활성화 된 애들이 한 명도 없으면 다 죽은거
        int length = group.GetComponentsInChildren<Monster>(false).Length;
        if (length == 0)
            allDead = true;

        if (allDead)
        {
            // 풀에 돌려주기
            groupPools[groupKey].Release(group);
            StartCoroutine(RespawnAfterDelay(groupKey));
        }
    }

    private IEnumerator RespawnAfterDelay(Vector3 groupKey)
    {
        yield return new WaitForSeconds(respawnDelay);
        Debug.Log("몬스터 재생성");
        SpawnGroupAt(groupKey);
    }
}
