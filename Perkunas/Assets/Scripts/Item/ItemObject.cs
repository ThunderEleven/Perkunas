using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable  // ��ȣ�ۿ� �������̽�
{
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // �ڿ� ������ ������ ����
        // �ڿ� ä�� �÷��̾� �κ��丮�� �߰�
    }
}
