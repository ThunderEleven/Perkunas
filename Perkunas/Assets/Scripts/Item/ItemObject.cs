using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable  // 상호작용 인터페이스
{
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // 자원 아이템 데이터 저장
        // 자원 채취 플레이어 인벤토리에 추가
    }
}
