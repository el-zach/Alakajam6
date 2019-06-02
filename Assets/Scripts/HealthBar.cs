using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;
    public Image healthBar;
    public Text nameText;
    //Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        health.OnDamage.AddListener(UpdateBar);
        UpdateSizeOfBar();
        //offset = transform.position - health.transform.position;
    }

    // Update is called once per frame
    

    void UpdateBar()
    {
        healthBar.rectTransform.localScale = new Vector3(health.currentHealth / health.maxHealth, 1, 1);
    }

    void UpdateSizeOfBar()
    {
        float size = health.maxHealth;
        //transform.localScale = new Vector3 (Remap(size, 1f, 20f, 0.0125f,3f),0.65f,1f);
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


}
