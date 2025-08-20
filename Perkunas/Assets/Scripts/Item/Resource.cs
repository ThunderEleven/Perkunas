using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Ore
}

public class Resource : MonoBehaviour, IInteractable
{
    [Header("Resource Info")]
    public ResourceType type;            // �ڿ� ����
    public int capacity = 5;             // �� ĳ�� Ƚ��(0�Ǹ� ��)
    public int dropCountPerHit = 1;      // �� �� Ķ �� ����Ǵ� ����
    public float respawnTime = 30f;      // ���� �ð�

    [Header("Drop Settings")]
    public ItemData dropItemData;        // ����� ������ ������
    public DropObjectPool dropPool;    // �� ����� ������ Ǯ(��� �����տ�)
    public float dropOffsetUp = 0.5f;    // ���ҽ����� �󸶳� ����� ����߸���
    public float dropSpreadRadius = 0.3f;// �ణ�� ���� ��ġ ����

    private int _currentCapacity;        // ���� ĳ�� Ƚ��

    // ���ҽ��� ĺ���� ��Ȱ��ȭ Ȱ��ȭ�ǰ�
    private Collider _collider;                 // ���ҽ� �ݶ��̴�
    private Renderer[] _renderers;              // ��������

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _renderers = GetComponentsInChildren<Renderer>();
    }

    private void OnEnable()
    {
        _currentCapacity = capacity; // ���� �� ĳ��Ƚ�� ����
        EnableResource(true);   // ������ �ݶ��̴� Ȱ��ȭ
    }

    public void OnInteract()
    {
        // �⺻������ ���ҽ� �ֺ��� ���
        HarvestAt(transform.position + Vector3.up * dropOffsetUp, Vector3.up);
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)         // ��Ʈ���� �ޱ�
    {

        //HarvestAt(hitPoint + hitNormal * dropOffsetUp, hitNormal);
    }

    // ���� ��� ó�� ����
    private void HarvestAt(Vector3 basePoint, Vector3 normal)
    {
        if (_currentCapacity <= 0)
            return;

        _currentCapacity--;

        // �� �� Ķ �� ���� �� ��� ����
        for (int i = 0; i < dropCountPerHit; i++)
        {
            if (dropPool == null || dropItemData == null)
            {
                Debug.LogWarning("[Resource] dropPool �Ǵ� dropItemData�� �������� ����.");
                break;
            }

            // �ణ ���� ��ġ�� �۶߸���
            Vector2 rand = Random.insideUnitCircle * dropSpreadRadius;
            Vector3 spawnPos = basePoint + new Vector3(rand.x, 0f, rand.y);

            GameObject drop = dropPool.Get(spawnPos, Quaternion.identity);
            ItemObject io = drop.GetComponent<ItemObject>();
            if (io != null)
            {
                io.SetItem(dropItemData, 1); // �� ���� ���(���ϸ� ���� ����)
            }
        }

        // ��ĳ�� ��Ȱ�� + ���� �ڷ�ƾ
        if (_currentCapacity <= 0)
        {
            EnableResource(false);
            StartCoroutine(RespawnAfter(respawnTime));
        }
    }

    private void EnableResource(bool enable)    // ���ҽ� �ݶ��̴�,������ Ȱ��ȭ, ��Ȱ��ȭ �޼���
    {
        if (_collider != null) _collider.enabled = enable;
        foreach (var r in _renderers)
        {
            r.enabled = enable;
        }
    }

    private IEnumerator RespawnAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _currentCapacity = capacity;
        EnableResource(true);  // ���ҽ� ������ Ȱ��ȭ
        Debug.Log($"[Resource] {gameObject.name} ������");
    }

    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }
}
