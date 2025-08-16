using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private float cellWidth = 5; // 셀 너비
    [SerializeField] private float cellHeight = 5; // 셀 높이

    [SerializeField] private bool visualiseGrid; // 그리드 시각화 여부
    [SerializeField] private int distanceFromPlayer = 5; // 플레이어로부터의 거리
    [SerializeField] private float gizmoSize = 0.2f; // 기즈모 크기

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        // 그리드 시각화 여부
        if (!visualiseGrid || !Application.isPlaying)
            return;

        Gizmos.color = Color.white;


        for (float x = -distanceFromPlayer; x <= distanceFromPlayer; x += cellWidth)
        {
            // 
            for (float y = -distanceFromPlayer; y <= distanceFromPlayer; y += cellHeight)
            {
                for (float z = -distanceFromPlayer; z <= distanceFromPlayer; z += cellHeight)
                {
                    // z축에 대한 그리드 위치 계산
                    Vector3 position = GetNearestGridPosition(transform.position) + new Vector3(x, y, z);
                    Gizmos.DrawCube(position, Vector3.one * gizmoSize);
                }
            }
        }
    }

    // 주어진 위치에 가장 가까운 그리드 위치를 반환합니다.
    private Vector3 GetNearestGridPosition(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellWidth) * cellWidth;
        float y = Mathf.Round(position.y / cellHeight) * cellHeight;
        float z = Mathf.Round(position.z / cellWidth) * cellWidth;

        return new Vector3(x, y, z);
    }
}
