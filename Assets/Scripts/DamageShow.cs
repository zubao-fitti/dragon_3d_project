using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageShow : MonoBehaviour
{
    TextMeshPro tmp;
    public float lifeTime = 2f;
    float currentTime = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= lifeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public void SetInfo(int value, Color colorText)
    {
        tmp = GetComponent<TextMeshPro>();
        tmp.SetText("{0}", value);
        tmp.color = colorText;
    }
}
