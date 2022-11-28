using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI TimeText;

    public GameObject panel;

    public GameObject clickButton;

    public float duration;
    public float currentTime;

    public TextMeshProUGUI ClicksTotalText;

    public TextMeshProUGUI HighScore;

    float TotalClicks;

    IEnumerator TimeIEn()
    {
        while (currentTime >= 0)
        {
            TimeText.text = currentTime.ToString();
            yield return new WaitForSeconds(1);
            currentTime--;
        }
        OpenPanel();
    }

    void OpenPanel()
    {
        TimeText.text = "";
        panel.SetActive(true);
    }
    
    public void AddClicks()
    {
        TotalClicks++;
        ClicksTotalText.text = TotalClicks.ToString("0");
    }

    private void Start()
    {
        panel.SetActive(false);
        currentTime = duration;
        TimeText.text = currentTime.ToString();
        StartCoroutine(TimeIEn());
        clickButton.SetActive(true);
    }

    private void Update()
    {
        ClicksTotalText.text = TotalClicks.ToString("0");
        if(currentTime == 0)
        {
            clickButton.SetActive(false);
            HighScore.text = "HIGHSCORE: " + TotalClicks.ToString();
          
        }
    }
}
