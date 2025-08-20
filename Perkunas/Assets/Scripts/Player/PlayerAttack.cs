using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹��" + collision.gameObject.name);
        // �ε��� ������Ʈ�� �±װ� "Monster"���� Ȯ��
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (collision.gameObject.TryGetComponent(out Monster monster))
            {
                monster.TakeDamage(10);
                Debug.Log("������ ����");
            }
        }
    }
}