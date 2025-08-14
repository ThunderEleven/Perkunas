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
    public int maxHealth = 3;   // �ڿ� ü��
    public int capacity;        // ����� ��

    void Start()
    {
        // ���� ��ġ
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)    // ä��
    {
        for(int i = 0; i < maxHealth; i++)
        {
            if (capacity <= 0)
                break;
            capacity -= 1;

            //Instantiate( , , );    // ä���Ȱ� ����
        }

        if (capacity <= 0)
        {
            Destroy(gameObject);     // ä�� �ٵǸ� �μ��� or ��Ȱ��ȭ
        }
    }
}
