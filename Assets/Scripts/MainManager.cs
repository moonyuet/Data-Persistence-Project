using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject titleScreen;
    public GameObject StartMenu;
    public GameObject gameElement;
    public TextMeshProUGUI nameInputField;

    private int _highScore;
    private string _highScorePlayerName;
    public Text HighScoreText;

    private bool m_Started = false;
    private string m_Player;
    private int m_Points;
    
    private bool m_GameOver = false;


    // Start is called before the first frame update
    public void StartGame()
    {
        if (!string.IsNullOrEmpty(nameInputField.text))
        {
            m_Player = nameInputField.text;
        }
        StartMenu.gameObject.SetActive(false);
        gameElement.gameObject.SetActive(true);
        titleScreen.gameObject.SetActive(true);

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        // Update high score display if needed
        var (highScore, highScoreName) = GetHighScore();
        HighScoreText.text = $"High Score: {highScore} Name: {highScoreName}";
        CheckHighScore();
    }

    public void CheckHighScore()
    {
        if (m_Points > _highScore)
        {
            _highScore = m_Points;
            _highScorePlayerName = m_Player;
            SaveHighScore();
        }
    }
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", _highScore);
        PlayerPrefs.SetString("HighScorePlayerName", _highScorePlayerName);
        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        _highScorePlayerName = PlayerPrefs.GetString("HighScorePlayerName", "None");
    }

    public (int score, string name) GetHighScore()
    {
        return (_highScore, _highScorePlayerName);
    }
}
