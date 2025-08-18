using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;   // ��������� ������
    public int initialSize = 10;// ���� ����

    private List<GameObject> _pool = new List<GameObject>();    // Ǯ ����Ʈ

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateOne();        // �ϳ� ����
        }
    }

    private void CreateOne()           // ������ ����
    {
        GameObject obj = Instantiate(prefab, transform);
        var marker = obj.GetComponent<PooledItem>();        // � Ǯ���� ��Ŀ
        if(marker == null)  // ��Ŀ������ ��Ŀ �߰�
        {
            marker = obj.AddComponent<PooledItem>();
        }

        marker.owner = this;    // ��Ŀ�� owner�� �� Ǯ�� ����

        obj.SetActive(false);   // Ǯ�� �ֱ��� ��Ȱ��ȭ
        _pool.Add(obj);         // ����Ʈ�� �߰�
    }

    public GameObject Get(Vector3 position, Quaternion rotation)    // �ʿ��� ��ġ, ȸ�������� ����
    {
        GameObject obj = null;

        for (int i = 0; i < _pool.Count; i++)   // ��Ȱ��ȭ ������Ʈ ã��
        {
            if (!_pool[i].activeSelf)   // !activeSelf = ������
            {
                obj = _pool[i];
                break;
            }
        }

        if (obj == null)            // ������Ʈ�� �����
        {
            CreateOne();            // �ϳ��� ����
            obj = _pool[_pool.Count - 1];
        }

        obj.transform.SetPositionAndRotation(position, rotation);   // ��ġ �ʱ�ȭ
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)  // ����� ���� ������Ʈ Ǯ�� ��ȯ
    {
        if (obj == null)
            return;

        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}
