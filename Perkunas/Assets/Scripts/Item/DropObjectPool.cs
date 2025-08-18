using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;   // 드랍아이템 프리펩
    public int initialSize = 10;// 생성 개수

    private List<GameObject> _pool = new List<GameObject>();    // 풀 리스트

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateOne();        // 하나 생성
        }
    }

    private void CreateOne()           // 프리팹 생성
    {
        GameObject obj = Instantiate(prefab, transform);
        var marker = obj.GetComponent<PooledItem>();        // 어떤 풀인지 마커
        if(marker == null)  // 마커없으면 마커 추가
        {
            marker = obj.AddComponent<PooledItem>();
        }

        marker.owner = this;    // 마커의 owner를 이 풀로 설정

        obj.SetActive(false);   // 풀에 넣기전 비활성화
        _pool.Add(obj);         // 리스트에 추가
    }

    public GameObject Get(Vector3 position, Quaternion rotation)    // 필요한 위치, 회전값으로 꺼냄
    {
        GameObject obj = null;

        for (int i = 0; i < _pool.Count; i++)   // 비활성화 오브젝트 찾기
        {
            if (!_pool[i].activeSelf)   // !activeSelf = 비사용중
            {
                obj = _pool[i];
                break;
            }
        }

        if (obj == null)            // 오브젝트가 사용중
        {
            CreateOne();            // 하나더 생성
            obj = _pool[_pool.Count - 1];
        }

        obj.transform.SetPositionAndRotation(position, rotation);   // 위치 초기화
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)  // 사용이 끝난 오브젝트 풀로 반환
    {
        if (obj == null)
            return;

        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}
