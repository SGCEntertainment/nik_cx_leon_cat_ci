using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    const int maxLines = 4;
    const string savekey = "leaders";

    [SerializeField] GameObject menu;
    [SerializeField] GameObject game;
    [SerializeField] GameObject gamePaused;
    [SerializeField] GameObject bestResults;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject gameOver;

    [Space(10)]
    [SerializeField] GameObject enviroment;

    [Space(10)]
    [SerializeField] Image pauseImg;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite playSprite;

    [Space(10)]
    [SerializeField] Transform healthBar;

    [Space(10)]
    [SerializeField] Sprite full;
    [SerializeField] Sprite empty;

    [Space(10)]
    [SerializeField] Text scoreText;

    [Space(10)]
    [SerializeField] Text leadersText;

    Leaderboard_data leaderboard_Data = new Leaderboard_data(maxLines);

    private void Start()
    {
        Open(Windows.Menu);
    }

    public void Open(Windows windows)
    {
        SoundManager.Instance.PlayEffect(0);

        switch(windows)
        {
            case Windows.Menu:

                Manager.IsStarted = false;
                enviroment.SetActive(false);
                game.SetActive(false);
                gamePaused.SetActive(false);
                bestResults.SetActive(false); 
                settings.SetActive(false); 
                menu.SetActive(true);
                gameOver.SetActive(false);
                break;

            case Windows.Game:

                Time.timeScale = 1;
                Manager.IsStarted = true;
                Manager.IsPaused = false;

                Manager.Instance.StartGame();

                enviroment.SetActive(true);
                pauseImg.sprite = Manager.IsPaused ? pauseSprite : playSprite;
                menu.SetActive(false); 
                gamePaused.SetActive(false); 
                bestResults.SetActive(false); 
                settings.SetActive(false); 
                game.SetActive(true);
                gameOver.SetActive(false);
                break;

            case Windows.Game_Paused:

                Time.timeScale = 0;
                Manager.IsStarted = false;
                Manager.IsPaused = !Manager.IsPaused;

                pauseImg.sprite = Manager.IsPaused ? pauseSprite : playSprite;
                menu.SetActive(false); 
                gamePaused.SetActive(true); 
                bestResults.SetActive(false); 
                settings.SetActive(false); 
                game.SetActive(true);
                gameOver.SetActive(false);
                break;

            case Windows.Unpaused:

                Time.timeScale = 1;
                Manager.IsStarted = true;
                Manager.IsPaused = !Manager.IsPaused;

                pauseImg.sprite = Manager.IsPaused ? pauseSprite : playSprite;
                menu.SetActive(false); 
                gamePaused.SetActive(false);
                bestResults.SetActive(false); 
                settings.SetActive(false);
                game.SetActive(true);
                gameOver.SetActive(false);
                break;

            case Windows.Best_results:

                LoadResults();
                Manager.IsStarted = false;
                enviroment.SetActive(false);
                menu.SetActive(false); 
                gamePaused.SetActive(false);
                bestResults.SetActive(true); 
                settings.SetActive(false);
                game.SetActive(false);
                gameOver.SetActive(false);
                break;

            case Windows.Settings:

                Manager.IsStarted = false;
                enviroment.SetActive(false);
                menu.SetActive(false); 
                gamePaused.SetActive(false); 
                bestResults.SetActive(false); 
                settings.SetActive(true); 
                game.SetActive(false);
                gameOver.SetActive(false);
                break;

            case Windows.Game_over:

                Manager.IsStarted = false;
                enviroment.SetActive(false);
                menu.SetActive(false);
                gamePaused.SetActive(false);
                bestResults.SetActive(false);
                settings.SetActive(false);
                game.SetActive(false);
                gameOver.SetActive(true);
                break;
        }
    }

    public void ResetHealth()
    {
        foreach (Transform t in healthBar)
        {
            Image _img = t.GetComponent<Image>();
            _img.sprite = full;
            _img.SetNativeSize();
        }

        Manager.Instance.healthCount = healthBar.childCount;
    }

    public void TakeDamage()
    {
        Manager.Instance.healthCount--;
        if (Manager.Instance.healthCount <= 0)
        {
            SaveResult();

            Open(Windows.Game_over);
        }

        healthBar.GetChild(Manager.Instance.healthCount).GetComponent<Image>().sprite = empty;
    }

    public void UpdateScore(int scoreAmount)
    {
        scoreText.text = string.Format("score: {0}", scoreAmount);
    }

    public void LoadResults()
    {
        leaderboard_Data = PlayerPrefs.HasKey(savekey) ? JsonUtility.FromJson<Leaderboard_data>(PlayerPrefs.GetString(savekey)) : new Leaderboard_data(maxLines);
        var newlist = leaderboard_Data.result.OrderByDescending(i => i);
        leaderboard_Data.result = newlist.ToList();

        leadersText.text = string.Empty;
        for (int i = 0; i < maxLines; i++)
        {
            leadersText.text += string.Format("{0}.  {1}  points", i + 1, leaderboard_Data.result[i]);
            if (i < maxLines - 1)
            {
                leadersText.text += "\n";
            }
        }
    }

    public void SaveResult()
    {
        leaderboard_Data.result.Add(Mathf.FloorToInt(Manager.Instance.scoreCount));

        var newlist = leaderboard_Data.result.OrderByDescending(i => i);
        leaderboard_Data.result = newlist.ToList();

        string data = JsonUtility.ToJson(leaderboard_Data);
        PlayerPrefs.SetString(savekey, data);
        PlayerPrefs.Save();

    }

    public void ResetResults()
    {
        SoundManager.Instance.PlayEffect(0);
        PlayerPrefs.DeleteKey(savekey);
        LoadResults();
    }

    [System.Serializable]
    public class Leaderboard_data
    {
        public List<int> result = new List<int>();

        public Leaderboard_data(int maxCount)
        {
            for (int i = 0; i < maxCount; i++)
            {
                result.Add(0);
            }
        }
    }
}
