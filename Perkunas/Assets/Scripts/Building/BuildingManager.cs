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

    [SerializeField] private Material transparentMat; // 투명 재질
    [SerializeField] private LayerMask buildLayer; // 빌드 레이어

    GameObject visualisedObjectType; // 시각화된 오브젝트 타입 GameObject -> item 변경
    Transform visualisedObject; // 시각화된 오브젝트 Transform

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

        if(visualisedObject == null || visualisedObjectType != obj)
        {
            StartVisualisingObject(obj); // 오브젝트 시각화 시작
        }

        VisualiseObject(pos, rotY, obj); // 오브젝트 시각화
    }

    // 오브젝트 시각화
    private void VisualiseObject(Vector3 pos, float rotY, GameObject obj)
    {
        bool isOccupied = false;

        Vector3 direction = (pos - transform.position).normalized; // 플레이어와 오브젝트 위치 간의 방향 벡터
        visualisedObject.position = pos + direction * -0.01f;
        visualisedObject.rotation = Quaternion.Euler(0, rotY, 0); // 시각화된 오브젝트 회전 설정


        visualisedObject.position = pos; // 시각화된 오브젝트 위치 설정
        visualisedObject.rotation = Quaternion.Euler(0, rotY, 0); // 시각화된 오브젝트 회전 설정

        Collider[] colliders = visualisedObject.GetComponentsInChildren<Collider>(); // 시각화된 오브젝트의 모든 콜라이더 가져오기
        foreach (Collider collider in colliders)
        {
            bool breakBothLoops = false; // 두 루프를 모두 중단할지 여부

            collider.isTrigger = true; // 모든 콜라이더를 트리거로 설정

            RaycastHit[] hits = Physics.BoxCastAll(collider.bounds.center, new Vector3(collider.bounds.size.x * 0.4f, collider.bounds.size.y * 0.4f,
collider.bounds.size.z * 0.4f), Vector3.up, Quaternion.identity, 1, buildLayer); // 박스 캐스트를 사용하여 충돌 검사

            foreach (RaycastHit hit in hits)
            {
                // BuiltObject가 있는지 확인
               /* if (hit.collider != null && hit.collider.GetComponentInParent<BuiltObject>() != null &&
    !obj.ignoreObjects.Contains(hit.collider.GetComponentInParent<BuiltObject>().objectType))*/
                {
                    isOccupied = true;
                    breakBothLoops = true;
                    break;
                }
            }

            if(breakBothLoops) // 두 루프를 모두 중단해야 하는 경우
            {
                break; // 외부 루프 중단
            }
        }

        Color color = isOccupied ? new Color(0.7f, 0.3f, 0.3f, 0.5f) : new Color(0.3f, 0.7f, 0.3f, 0.5f); // 충돌 여부에 따라 색상 설정
        transparentMat.color = color; // 투명 재질의 색상 설정

        if (Input.GetMouseButtonDown(0) && !isOccupied) // 마우스 왼쪽 버튼 클릭 시
        {
            BuildObject(pos, rotY, obj); // 오브젝트 배치
        }
    }

    // 오브젝트 시각화 시작
    private void StartVisualisingObject(GameObject obj)
    {
        visualisedObjectType = obj; // 시각화된 오브젝트 타입 설정

        if(visualisedObject != null)
        {
            Destroy(visualisedObject.gameObject); // 기존 시각화된 오브젝트 제거
        }

        visualisedObject = Instantiate(visualisedObjectType).transform; // 새로운 시각화된 오브젝트 생성 visualisedObjectType.prefab
        visualisedObject.localScale = new Vector3(cellWidth, cellHeight, cellWidth); // 시각화된 오브젝트 크기 조정

        MeshRenderer renderer = visualisedObject.GetComponentInChildren<MeshRenderer>();
        Material[] materials = new Material[renderer.materials.Length];

        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = transparentMat;
        }

        renderer.materials = materials; // 시각화된 오브젝트의 재질을 투명 재질로 설정

        visualisedObject.gameObject.layer = 2;

        foreach (Transform child in visualisedObject)
        {
            child.gameObject.layer = 21; // 자식 오브젝트의 레이어를 3으로 설정
        }
    }

    // 오브젝트 시각화 종료
    public void EndVisualisingObject()
    {
        if(visualisedObject != null)
        {
            Destroy(visualisedObject.gameObject); // 시각화된 오브젝트 제거

        }
    }


    private void BuildObject(Vector3 pos, float rotY, GameObject obj)
    {
        GameObject newObj = Instantiate(obj, pos, Quaternion.Euler(0, rotY, 0)); // 오브젝트 생성
        newObj.transform.localScale = new Vector3(cellWidth, cellHeight, cellWidth); // 오브젝트 크기 조정


        int layerIndex = Mathf.FloorToInt(Mathf.Log(buildLayer, 2)); // 레이어 인덱스 계산

        foreach (Transform child in newObj.transform)
        {
            child.gameObject.layer = layerIndex; // 자식 오브젝트의 레이어를 설정
        }

        newObj.layer = layerIndex; // 오브젝트의 레이어를 설정

        BuiltObject builtObject = newObj.GetComponent<BuiltObject>(); // BuiltObject 컴포넌트 가져오기
        // builtObject.objectType = obj;
    }
}



/*
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string name;
    public Sprite icon;
    public GameObject prefab;
    public int stackMax;
    public bool placable;
    public bool snapToGridEdge;
    public List<ItemSO> ignoreObjects;
}
*/