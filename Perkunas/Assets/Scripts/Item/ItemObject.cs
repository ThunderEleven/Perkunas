using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IInteractable  // 상호작용 인터페이스
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [Header("Runtime")]
    public ItemData data;
    public int quantity = 1;            // 한번에 한개씩 줍게

    [Header("Auto Despawn")]            // 드랍아이템 자동으로 사라지게
    public bool autoReturn = true;      // 일정 시간 지나면 자동 반납
    public float lifetime = 20f;        // 20초 뒤 삭제

    private float _timer;

    private void OnEnable()
    {
        _timer = 0f; // 활성화될 때 타이머 리셋
    }

    private void Update()
    {
        if (!autoReturn) return;

        _timer += Time.deltaTime;
        if (_timer >= lifetime)
        {
            ReturnToPool();     // 풀로 넣기
        }
    }

    public void SetItem(ItemData newData, int amount)   // 아이템 세팅
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
        // 인벤토리에 추가
        ReturnToPool();     // 풀로 넣기
    }

    private void ReturnToPool()
    {
        var pooled = GetComponent<PooledItem>();
        if (pooled != null && pooled.owner != null) // 풀이면
        {
            pooled.owner.Return(gameObject); // 풀로 되돌리기
        }
        else
        {
            Destroy(gameObject); // 아니면 파괴
        }
    }
}
