using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;   // 드랍아이템 프리펩
    public int initialSize = 10;// 생성 개수

    //private readonly Queue<GameObject> _queue   // 큐 재할당 하지 못하게 필드에 붙이는 안전장치
    //= new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            InstantiateOne();        // 하나 생성
        }
    }

    private void InstantiateOne()
    {
        GameObject obj = Instantiate(prefab, transform);
        var marker = obj.GetComponent<PooledItem>();
        if(marker == null)
        {
            marker = obj.AddComponent<PooledItem>();
        }
    }
}
