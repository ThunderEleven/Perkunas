using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcData : MonoBehaviour
{
    public string npcName;
    [SerializeField] private List<DialogueEntry> dialogue;
    
    private void Start()
    {
        npcName = transform.name;
        
        // 해당 npc의 이름을 가진 대화 데이터만 가져오도록 구현
        foreach (var data in NpcDataManager.Instance.Dialogues)
        {
            if (data.npcName == this.npcName)
            {
                dialogue.Add(data);
            }
        }
    }
    
    // 대화 내용 출력 테스트용 콜라이더 처리 메서드
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            NpcTalkManager.Instance.InitTalkingUI(dialogue);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            NpcTalkManager.Instance.RemoveTalkingUI();
        }
    }
}
