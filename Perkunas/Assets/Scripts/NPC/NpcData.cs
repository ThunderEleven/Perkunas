using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcData : MonoBehaviour, IInteractable
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
    
    public string GetInteractPrompt()
    {
        string str = "대화 하기";
        return str;
    }

    public void OnInteract()
    {
        UIManager.Instance.GetUI<UITalking>().InitTalkingUI(dialogue);
    }
}
