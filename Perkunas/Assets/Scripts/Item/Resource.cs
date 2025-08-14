using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Ore,
    Dirt
}

public class Resource : MonoBehaviour
{
    public ResourceType type;
    public int maxHealth = 3;   // 자원 체력
    public int capacity;        // 드랍템 양

    void Start()
    {
        // 스폰 위치
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)    // 채집
    {
        for(int i = 0; i < maxHealth; i++)
        {
            if (capacity <= 0)
                break;
            capacity -= 1;

            //Instantiate( , , );    // 채집된것 생성
        }

        if (capacity <= 0)
        {
            Destroy(gameObject);     // 채집 다되면 부수기 or 비활성화
        }
    }
}
