using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FinishScreen : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    float checkFinishTime = 10f;
    float checkTime = 0.0f;

    void Start()
    {
        checkTime = Time.time + checkFinishTime;
    }

    void Update()
    {
        if (Time.time > checkTime)
        {
            if (GameObject.FindWithTag("Enemy") == null)
            {
                ActiveElements();
                tmp.SetText("Victory");
                tmp.color = Color.green;
            }
            checkTime = Time.time + checkFinishTime;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void FinishGame()
    {
        ActiveElements();
        tmp.SetText("DEFEAT");
        tmp.color = Color.red;
    }

    void ActiveElements()
    {
        gameObject.GetComponent<Image>().enabled = true;
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
