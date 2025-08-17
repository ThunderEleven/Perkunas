using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;   // ��������� ������
    public int initialSize = 10;// ���� ����

    //private readonly Queue<GameObject> _queue   // ť ���Ҵ� ���� ���ϰ� �ʵ忡 ���̴� ������ġ
    //= new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            InstantiateOne();        // �ϳ� ����
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
