using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledItem : MonoBehaviour
{
    public DropObjectPool owner;

    // ������ �ݳ� ����: �ܺο��� �ٷ� ȣ���ϱ� ���ϵ��� ����
    public void ReturnToPool()
    {
        if (owner != null)                      // owner�� null�̾ƴϸ�
            owner.Return(gameObject);           // ���� Ǯ�� Return �޼��带 ȣ���ؼ� �ݳ�
        else
            Destroy(gameObject);
    }
}
