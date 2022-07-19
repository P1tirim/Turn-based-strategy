using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Quaternion startRotation;

    public Image healthBarImage;
    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = startRotation;
    }

    public void UpdateHealthBar(float healthCurrent, float healthMax)
    {
        healthBarImage.fillAmount = Mathf.Clamp(healthCurrent / healthMax, 0, 1f);
    }
}