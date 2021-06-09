using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public static string playerName;
    private TextMeshProUGUI highScoreText;
    public static int highScore;
    public static string highScorer;//Have to repeat for now bc MainManager spawns in the second scene, not at the beginning of the game. Dunno any other way around this, currently.

    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GameObject.Find("Current High Score Text").GetComponent<TextMeshProUGUI>();
        playerName = "Anonymous";
        highScore = -1;
        highScorer = "";
        LoadScore();
        if (highScore != -1 && highScorer != "")//Assuming a successful file load, the high score and scorer will change.
        {
            highScoreText.text = "Current High Score: " + highScore + " (" + highScorer + ")";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        if (playerName == "")//Name Input is handled in the NameInput() script. It changes the playerName static variable.
        {
            playerName = "Anonymous";
        }
        SceneManager.LoadScene(1);//Loads the main scene.
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public int totalPoints;
        public string name;
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.totalPoints;
            highScorer = data.name;
        }
    }
}
