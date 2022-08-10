using UnityEngine;

public class Manager : MonoBehaviour
{
    public static bool IsPaused;
    public static bool IsStarted;

    private static Manager instance;
    public static Manager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<Manager>();
            }

            return instance;
        }
    }

    const float ySpeed = 6;
    const float yMin = -8.84f;
    const float yOffset = 6.0f;

    [HideInInspector]
    public int healthCount;

    [HideInInspector]
    public int scoreCount;

    [SerializeField] Transform obstacles;
    public Player player;
    [SerializeField] Transform hand;

    private void Awake()
    {
        IsPaused = false;
        IsStarted = false;
    }

    private void Update()
    {
        if(!IsStarted)
        {
            return;
        }

        UpdateObstaclesPos();
    }

    void UpdateObstaclesPos()
    {
        foreach(Transform t in obstacles)
        {
            t.localPosition += ySpeed * Time.deltaTime * Vector3.down;

            if(t.position.y <= hand.position.y && t.gameObject.activeSelf)
            {
                InteractWithHand(t.gameObject);
            }

            if(t.position.y <= yMin)
            {
                if (!t.gameObject.activeSelf)
                {
                    t.gameObject.SetActive(true);
                }

                t.SetPositionAndRotation(new Vector3(GetXPos(), GetLastPosY() + yOffset, 0), Quaternion.Euler(0, 0, Random.Range(0, 360.0f)));
                t.SetAsLastSibling();
            }
        }
    }

    void InteractWithHand(GameObject obstacle)
    {
        obstacle.SetActive(false);

        Vector2 reltive = obstacle.transform.position - hand.position;
        bool nearPlane = Vector2.Dot(reltive, player.lookDirection) > 0;

        if (nearPlane)
        {
            if(obstacle.CompareTag("normal"))
            {
                AddScore();
                SoundManager.Instance.PlayEffect(1);
            }
            else if (obstacle.CompareTag("negative"))
            {
                UIManager.Instance.TakeDamage();
                SoundManager.Instance.PlayEffect(2);
            }
        }
    }

    void ResetObstacles()
    {
        foreach (Transform t in obstacles)
        {
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(true);
            }

            t.SetPositionAndRotation(new Vector3(GetXPos(), Mathf.Abs(yMin) + (t.GetSiblingIndex() * yOffset), 0), Quaternion.Euler(0, 0, Random.Range(0, 360.0f)));
        }
    }

    float GetLastPosY()
    {
        return obstacles.GetChild(obstacles.childCount - 1).position.y;
    }

    float GetXPos()
    {
        return Random.Range(0, 100) > 50 ? -1.5f : 1.5f;
    }

    public void StartGame()
    {
        scoreCount = 0;

        ResetObstacles();
        UIManager.Instance.UpdateScore(scoreCount);
        UIManager.Instance.ResetHealth();
    }

    public void AddScore()
    {
        scoreCount++;
        UIManager.Instance.UpdateScore(scoreCount);
    }

    public void GameOver()
    {
        UIManager.Instance.SaveResult();
        UIManager.Instance.Open(Windows.Game_over);
    }
}
