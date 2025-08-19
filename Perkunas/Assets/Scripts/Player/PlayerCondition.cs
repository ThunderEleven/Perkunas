using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    [Header("체력 관련")]
    public float curHp;      // 현재 체력
    public float maxHp;      // 최대 체력
    public float startHp;    // 시작 체력
    public float passiveHp;  // 자동 회복되는 체력
    public Image hpUIBar;
    public float noHungerHealthDecay;
    
    [Header("스테미나 관련")]
    public float curStamina;      // 현재 스태미나
    public float maxStamina;      // 최대 스태미나
    public float startStamina;    // 시작 스태미나
    public float passiveStamina;  // 자동 회복되는 스태미나
    public Image staminaUIBar;
    
    [Header("배고픔 관련")]
    public float curHunger;      // 현재 배고픔
    public float maxHunger;      // 최대 배고픔
    public float startHunger;    // 시작 배고픔
    public float passiveHunger;  // 자동으로 감소되는 배고픔
    public Image hungerUIBar;
    
    [Header("갈증 관련")]
    public float curThirst;      // 현재 갈증
    public float maxThirst;      // 최대 갈증
    public float startThirst;    // 시작 갈증
    public float passiveThirst;  // 자동으로 감소되는 갈증
    public Image thirstUIBar;

    public event Action onTakeDamage;
    
    private void Start()
    {
        curHp = startHp;
        curStamina = startStamina;
        curHunger = startHunger;
        curThirst = startThirst;
    }

    private void Update()
    {
        GetPercentage();
        CalculatePassiveValue();
    }

    // UI 업데이트
    private void GetPercentage()
    {
        hpUIBar.fillAmount = curHp / maxHp;
        staminaUIBar.fillAmount = curStamina / maxStamina;
        hungerUIBar.fillAmount = curHunger / maxHunger;
        thirstUIBar.fillAmount = curThirst / maxThirst;
    }

    // 각 컨디션의 passiveValue들을 계산하는 메서드
    private void CalculatePassiveValue()
    {
        SubtractHunger(passiveHunger * Time.deltaTime);
        AddStamina(passiveStamina * Time.deltaTime);

        if (curHunger <= 0f)
        {
            SubtractHp(noHungerHealthDecay * Time.deltaTime);
        }
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        SubtractHp(damageAmount);
        onTakeDamage?.Invoke();
    }

    // 각 컨디션 별로 add와 subtract 메서드들 구현 -> switch로 한번에 통합?
    public void AddHp(float amount)
    {
        curHp = Mathf.Min(curHp + amount, maxHp);
    }
    
    public void SubtractHp(float amount)
    {
        curHp = Mathf.Max(curHp - amount, 0.0f);
    }

    public void AddStamina(float amount)
    {
        curStamina = Mathf.Min(curStamina + amount, maxStamina);
    }

    public void SubtractStamina(float amount)
    {
        curStamina = Mathf.Max(curStamina - amount, 0.0f);
    }

    public void AddHunger(float amount)
    {
        curHunger = Mathf.Min(curHunger + amount, maxHunger);
    }

    public void SubtractHunger(float amount)
    {
        curHunger = Mathf.Max(curHunger - amount, 0.0f);
    }

    public void AddThirst(float amount)
    {
        curThirst = Mathf.Min(curThirst + amount, maxThirst);
    }

    public void SubtractThirst(float amount)
    {
        curThirst = Mathf.Max(curThirst - amount, 0.0f);
    }
}
