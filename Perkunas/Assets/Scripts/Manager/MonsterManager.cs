using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterManager : MonoSingleton<MonsterManager>
{
   public List<GameObject> groupPrefabs;
   private Dictionary<GameObject, ObjectPool<GameObject>> groupPool;
   public int maxPoolCount = 20;

   private void Awake()
   {
      groupPool = new Dictionary<GameObject, ObjectPool<GameObject>>();
      foreach (var group in groupPrefabs)
      {
         var pool = new ObjectPool<GameObject>(() =>
            {
               GameObject go = Instantiate(group);
               go.SetActive(false);
               return go;
            },
            go =>
            {
               go.SetActive(true);
            },
            go =>
            {
               go.SetActive(false);
            },
            go =>
            {
               Destroy(go);
            },
            true,
            group.GetComponentsInChildren<GameObject>().Length,
            maxPoolCount
         );
         groupPool.Add(group, pool);
      }
   }
}
