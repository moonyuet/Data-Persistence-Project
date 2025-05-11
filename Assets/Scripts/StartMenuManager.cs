using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Button button;
    public int options;
    private MainManager mainManager;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        mainManager.LoadHighScore();
        if (options > 0){

            Quit();

        }
        else
        {
            mainManager.StartGame();
        }

   
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
