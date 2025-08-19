using UnityEngine;

public class ResourceTest : MonoBehaviour
{
    private Resource resource;

    private void Start()
    {
        resource = GetComponent<Resource>();
        if (resource == null)
        {
            Debug.LogError("Resource ������Ʈ�� ����!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // �����̽��ٷ� �׽�Ʈ
        {
            if (resource != null)
            {
                resource.OnInteract();
                Debug.Log("Resource OnInteract ����� (��� Ȯ��)");
            }
        }
    }
}