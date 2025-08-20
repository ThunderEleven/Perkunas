using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTeleport : MonoBehaviour
{
    public Transform target;

    public float teleportDistance = 2.0f;  // 순간이동할곳 안겹치게

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 newPlayerPosition = target.position + target.TransformDirection(Vector3.forward) * teleportDistance;    // 앞으로 좀 더 나오게

            collision.gameObject.transform.position = newPlayerPosition;    // 순간이동
        }
    }
}
