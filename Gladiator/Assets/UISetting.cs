using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public GameObject panelPause;
    public GameObject panelWin;
    public GameObject panelLose;
    int trophy;
    int coin;
    public Text trophyWin;
    public Text coinWin;
    public Text trophyLose;

    private void Start()
    {
        trophy = PlayerPrefs.GetInt("trophy");
        coin = PlayerPrefs.GetInt("coin");
        OffPause();
    }
    public void Pause()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0;
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OffPause()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1;
    }
    public void Lose()
    {
        int trophyMinus = Random.Range(7, 12)/* * PlayerPrefs.GetInt("index")*/;

        trophyLose.text = "-" + trophyMinus;
        if (trophy - trophyMinus > 0)
            trophy -= trophyMinus;
        PlayerPrefs.SetInt("trophy", trophy);
        panelLose.SetActive(true);
    }
    public void Win()
    {
        int trophyPlus = Random.Range(7, 12)/* * PlayerPrefs.GetInt("index")*/;
        int coinPlus = Random.Range(10, 15)/* * PlayerPrefs.GetInt("index")*/;

        trophyWin.text = "+" + trophyPlus;
        coinWin.text = "+" + coinPlus;

        trophy += trophyPlus;
        coin += coinPlus;

        PlayerPrefs.SetInt("trophy", trophy);
        PlayerPrefs.SetInt("coin", coin);

        panelWin.SetActive(true);
    }
}
