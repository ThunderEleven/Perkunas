using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IInteractable  // ��ȣ�ۿ� �������̽�
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [Header("Runtime")]
    public ItemData data;
    public int quantity = 1;            // �ѹ��� �Ѱ��� �ݰ�

    [Header("Auto Despawn")]            // ��������� �ڵ����� �������
    public bool autoReturn = true;      // ���� �ð� ������ �ڵ� �ݳ�
    public float lifetime = 20f;        // 20�� �� ����

    private float _timer;

    private void OnEnable()
    {
        _timer = 0f; // Ȱ��ȭ�� �� Ÿ�̸� ����
    }

    private void Update()
    {
        if (!autoReturn) return;

        _timer += Time.deltaTime;
        if (_timer >= lifetime)
        {
            ReturnToPool();     // Ǯ�� �ֱ�
        }
    }

    public void SetItem(ItemData newData, int amount)   // ������ ����
    {
        data = newData;
        quantity = amount;
    }

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}x{quantity}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // �κ��丮�� �߰�
        ReturnToPool();     // Ǯ�� �ֱ�
    }

    private void ReturnToPool()
    {
        var pooled = GetComponent<PooledItem>();
        if (pooled != null && pooled.owner != null) // Ǯ�̸�
        {
            pooled.owner.Return(gameObject); // Ǯ�� �ǵ�����
        }
        else
        {
            Destroy(gameObject); // �ƴϸ� �ı�
        }
    }
}
