using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        if (MenuUIHandler.highScore != -1)//If you successfully load a high score
        {
            bestScoreText.text = "Best Score: " + MenuUIHandler.highScore + " (" + MenuUIHandler.highScorer + ")";
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//This is where the scene restarts on the press of the Spacebar.
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score: {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > MenuUIHandler.highScore)//If there's a new high score, update stuff
        {
            bestScoreText.text = "Best Score: " + m_Points + " (" + MenuUIHandler.playerName + ")";
            SaveScore();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public int totalPoints;
        public string name;
    }

    public void SaveScore()//Only applied if a high score at the end of the game.
    {
        SaveData data = new SaveData();
        data.totalPoints = m_Points;
        data.name = MenuUIHandler.playerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            MenuUIHandler.highScore = data.totalPoints;
            MenuUIHandler.highScorer = data.name;//Interestingly enough, even though it's a new scene and MenuUIHandler itself may not be in the scene, its static variables can still be referenced!
        }
    }
}
