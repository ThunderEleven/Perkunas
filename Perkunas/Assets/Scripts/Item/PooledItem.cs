using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledItem : MonoBehaviour
{
    public DropObjectPool owner;

    // 간단한 반납 헬퍼: 외부에서 바로 호출하기 편하도록 제공
    public void ReturnToPool()
    {
        if (owner != null)                      // owner가 null이아니면
            owner.Return(gameObject);           // 소유 풀의 Return 메서드를 호출해서 반납
        else
            Destroy(gameObject);
    }
}
