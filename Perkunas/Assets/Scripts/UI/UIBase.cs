using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnOpen()
    {
        
    }

    protected virtual void OnClose()
    {
        
    }
}
