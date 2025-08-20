using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌함" + collision.gameObject.name);
        // 부딪힌 오브젝트의 태그가 "Monster"인지 확인
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (collision.gameObject.TryGetComponent(out Monster monster))
            {
                monster.TakeDamage(10);
                Debug.Log("데미지 입음");
            }
        }
    }
}