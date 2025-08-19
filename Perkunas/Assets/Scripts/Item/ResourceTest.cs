using UnityEngine;

public class ResourceTest : MonoBehaviour
{
    private Resource resource;

    private void Start()
    {
        resource = GetComponent<Resource>();
        if (resource == null)
        {
            Debug.LogError("Resource 컴포넌트가 없음!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // 스페이스바로 테스트
        {
            if (resource != null)
            {
                resource.OnInteract();
                Debug.Log("Resource OnInteract 실행됨 (드랍 확인)");
            }
        }
    }
}