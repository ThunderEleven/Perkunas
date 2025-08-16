using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;          // 풀링할거에 태그
        public GameObject prefab;
        public int size;            // 생성 갯수
    }

    public List<Pool> pools = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private Dictionary<string, GameObject> prefabMap = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this) // 이미 다른 인스턴스가 있으면
        {
            Destroy(gameObject);                  // 중복 방지용 파괴
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);            // 씬 전환 시에도 유지
    }

    private void Start()                          // 게임 시작 시 초기화
    {
        // 풀 사전 초기화
        foreach (var pool in pools)               // 등록된 모든 풀에 대해
        {
            // 태그별 비활성 큐 생성
            var objectQueue = new Queue<GameObject>();

            // 초기 사이즈만큼 미리 만든다 (Warm-up)
            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, poolRoot); // 프리팹을 생성, 정리용 부모에 귀속
                obj.SetActive(false);                         // 비활성화하여 풀에 보관
                // 이 오브젝트가 어느 풀에 속하는지 저장하는 컴포넌트 부착
                var marker = obj.GetComponent<PooledObject>();
                if (marker == null) marker = obj.AddComponent<PooledObject>();
                marker.tagInPool = pool.tag;                  // 자신의 풀 태그 기록
                objectQueue.Enqueue(obj);                     // 큐에 집어넣음
            }

            poolDictionary[pool.tag] = objectQueue;           // 태그 -> 큐 등록
            prefabMap[pool.tag] = pool.prefab;                // 태그 -> 프리팹 등록(추가 생성용)
        }
    }

    void Update()
    {
        
    }
}
