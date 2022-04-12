using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour, IPointerDownHandler
{
    public Text trophy;
    public Text coin;

    public int[] trophyMinLevel;
    public float[] trophyMaxLevel;

    public Image LevelProgress;
    public Text LevelIndex;
    public Text ArenaText;
    public Image ImageMap;
    public Sprite[] map;
    int level;
    public bool scene = false;

    private void Start()
    {
        if (trophy != null)
        {
            trophy.text = PlayerPrefs.GetInt("trophy").ToString();
            coin.text = PlayerPrefs.GetInt("coin").ToString();
        }
        if (LevelProgress != null)
        {
            if (PlayerPrefs.HasKey("level"))
                level = PlayerPrefs.GetInt("level");
            else
                level = 1;
            if (PlayerPrefs.GetInt("trophy") > trophyMinLevel[level] && PlayerPrefs.GetInt("trophy") < trophyMaxLevel[level])
            {
                level++;
            }
            else if (PlayerPrefs.GetInt("trophy") < trophyMinLevel[level])
                level--;
            PlayerPrefs.SetInt("level", level);
            LevelIndex.text = level.ToString();
            ArenaText.text = "Arena" + level.ToString();
            ImageMap.sprite = map[level - 1];
            LevelProgress.fillAmount = (PlayerPrefs.GetInt("trophy") - trophyMinLevel[level - 1]) / trophyMaxLevel[level - 1];
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PlayerPrefs.DeleteAll();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (scene == true)
            SceneManager.LoadScene(name);
        if(name == "Arena")
        {
            SceneManager.LoadScene("Arena" + level.ToString());
        }
    }
}
