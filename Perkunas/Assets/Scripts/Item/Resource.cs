using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum ResourceType
{
    Wood,
    Stone,
    Ore,
}

public class Resource : MonoBehaviour
{
    [Header("Resource Info")]
    public ResourceType type;            // 자원 종류
    public int capacity = 5;             // 총 캐는 횟수(0되면 고갈)
    public int dropCountPerHit = 1;      // 한 번 캘 때 드랍되는 개수
    public float respawnTime = 30f;      // 리젠 시간

    [Header("Drop Settings")]
    public ItemData dropItemData;        // 드랍할 아이템 데이터
    public DropObjectPool dropPool;    // 이 드랍을 관리할 풀(드랍 프리팹용)
    public float dropOffsetUp = 0.5f;    // 리소스에서 얼마나 띄워서 떨어뜨릴지
    public float dropSpreadRadius = 0.3f;// 약간의 랜덤 위치 퍼짐

    private int _currentCapacity;        // 현재 캐는 횟수
    private void OnEnable()
    {
        _currentCapacity = capacity; // 리젠 시 캐는횟수 리젠
    }

    public void OnInteract()
    {
        // 기본값으로 리소스 주변에 드랍
        HarvestAt(transform.position + Vector3.up * dropOffsetUp, Vector3.up);
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)         // 히트지점 받기
    {

        //HarvestAt(hitPoint + hitNormal * dropOffsetUp, hitNormal);
    }

    // 실제 드랍 처리 로직
    private void HarvestAt(Vector3 basePoint, Vector3 normal)
    {
        if (_currentCapacity <= 0)
            return;

        _currentCapacity--;

        // 한 번 캘 때 여러 개 드랍 가능
        for (int i = 0; i < dropCountPerHit; i++)
        {
            if (dropPool == null || dropItemData == null)
            {
                Debug.LogWarning("[Resource] dropPool 또는 dropItemData가 설정되지 않음.");
                break;
            }

            // 약간 랜덤 위치로 퍼뜨리기
            Vector2 rand = Random.insideUnitCircle * dropSpreadRadius;
            Vector3 spawnPos = basePoint + new Vector3(rand.x, 0f, rand.y);

            GameObject drop = dropPool.Get(spawnPos, Quaternion.identity);
            ItemObject io = drop.GetComponent<ItemObject>();
            if (io != null)
            {
                io.SetItem(dropItemData, 1); // 한 개씩 드랍(원하면 수량 조절)
            }
        }

        // 다캐면 비활성 + 리젠 코루틴
        if (_currentCapacity <= 0)
        {
            gameObject.SetActive(false);
            StartCoroutine(RespawnAfter(respawnTime));
        }
    }

    private IEnumerator RespawnAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(true); // OnEnable에서 용량 리셋됨
    }
}
