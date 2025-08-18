using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcData : MonoBehaviour
{
    public string npcName;
    private List<Dictionary<string, object>> dialogue;
    
    private void Start()
    {
        dialogue = CSVReader.Read("DialogueData");

        for (int i = 0; i < dialogue.Count; i++)
        {
            Debug.Log("대화 번호: " + dialogue[i]["대화 번호"]);
            Debug.Log("NPC 이름: " + dialogue[i]["npc 이름"]);
            Debug.Log("대화 내용: " + dialogue[i]["대화 내용"]);
        }
    }
}
