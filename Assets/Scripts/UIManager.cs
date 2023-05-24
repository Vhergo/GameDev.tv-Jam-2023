using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fillGradual;
    [SerializeField] private Image fillInstant;
    [SerializeField] private float XPIncreaseRate;
    private float gradualIncreaseTarget = 0;

    void Update() {
        fillGradual.fillAmount = Mathf.MoveTowards(fillGradual.fillAmount, gradualIncreaseTarget, XPIncreaseRate * Time.deltaTime);
        
        if (fillGradual.fillAmount >= 1) {
            fillGradual.fillAmount = 0;
            fillInstant.fillAmount = 1;
        }
    }

    public void UpdateXPBar(float maxXP, float currentXP) {
        float updatedXP = currentXP / maxXP;
        fillInstant.fillAmount = updatedXP;
        gradualIncreaseTarget = updatedXP;

        if (gradualIncreaseTarget < fillGradual.fillAmount) fillGradual.fillAmount = gradualIncreaseTarget;
    }
}
