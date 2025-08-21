using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    float rotY;
    // 오브젝트 배치
    public void PlaceObject(Vector3 pos, GameObject obj)
    {
        Vector3 basePos = pos;
        if (Input.GetKeyDown(KeyCode.Q)) // 시계 방향 회전
            rotY -= 90;
        else if (Input.GetKeyDown(KeyCode.E)) // 반시계 방향 회전
            rotY += 90;

        pos = GetNearestGridPosition(pos); // 그리드에 맞게 위치 조정

        //if(obj.snapToGridEdge)
        //{
        //    Vector2 direction = new Vector2(basePos.x - pos.x, basePos.z - pos.z);
        //    float x = direction.x < 0 ? -1 : 1;
        //    float z = direction.y < 0 ? -1 : 1;

        //    if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        //    {
        //        rotY = 90;
        //        pos += new Vector3(0,0, z * cellWidth / 2); // Z축 방향으로 위치 조정
        //    }
        //    else
        //    {
        //        rotY = 0;
        //        pos += new Vector3(x * cellWidth / 2, 0, 0); // X축 방향으로 위치 조정
        //    }
        //}

        VisualiseObject(pos, rotY, obj); // 오브젝트 시각화
    }

    // 오브젝트 시각화
    private void VisualiseObject(Vector3 pos, float rotY, GameObject obj)
    {
        bool isOccupied = false;
        if (Input.GetMouseButtonDown(0) && !isOccupied) // 마우스 왼쪽 버튼 클릭 시
        {
            BuildObject(pos, rotY, obj); // 오브젝트 배치
        }
    }

    // 오브젝트 시각화 시작
    private void StartVisualisingObject(Vector3 pos, float rotY, GameObject obj)
    {

    }

    // 오브젝트 시각화 종료
    public void EndVisualisingObject()
    {

    }


    private void BuildObject(Vector3 pos, float rotY, GameObject obj)
    {
        GameObject newObj = Instantiate(obj, pos, Quaternion.Euler(0, rotY, 0)); // 오브젝트 생성
        newObj.transform.localScale = new Vector3(cellWidth, cellHeight, cellWidth); // 오브젝트 크기 조정
    }
}
