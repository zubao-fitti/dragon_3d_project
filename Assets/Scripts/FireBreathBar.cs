using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FireBreathBar : MonoBehaviour
{
    Image img;
    public TextMeshProUGUI tmp;
    public float skillRate;
    float skillTime;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.color = Color.red;
        tmp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > skillTime)
        {
            img.color = Color.red;
            tmp.enabled = false;
        }
        else
        {
            tmp.SetText("{0}", Mathf.Round(skillTime - Time.time));
        }
    }

    public void SetCooldown()
    {
        skillTime = Time.time + 1f / skillRate;

        tmp.enabled = true;
        tmp.SetText("{0}", skillTime - Time.time);

        img.color = Color.gray;
    }
}
