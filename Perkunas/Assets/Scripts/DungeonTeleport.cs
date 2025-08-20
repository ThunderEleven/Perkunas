using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTeleport : MonoBehaviour
{
    public Transform target;

    public float teleportDistance = 2.0f;  // �����̵��Ұ� �Ȱ�ġ��

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 newPlayerPosition = target.position + target.TransformDirection(Vector3.forward) * teleportDistance;    // ������ �� �� ������

            collision.gameObject.transform.position = newPlayerPosition;    // �����̵�
        }
    }
}
