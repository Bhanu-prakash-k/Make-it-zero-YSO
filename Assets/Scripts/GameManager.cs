using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject geniusPanel;

    [SerializeField] private TMP_Text levelText;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private int maxLevels;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        geniusPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        losePanel.SetActive(false);
        nextButton.onClick.AddListener(NextButtonClick);
        restartButton.onClick.AddListener(RestartButtonClick);
        maxLevels = LevelLoader.instance.levels.Length - 1;
        levelText.text = "Level - " + (LevelLoader.instance.nextButtonClickedValue + 1).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextButtonClick()
    {
        if(LevelLoader.instance.nextButtonClickedValue < maxLevels)
        {
            PlayerPrefs.SetInt("NextButtonClicked", LevelLoader.instance.nextButtonClickedValue + 1);
            nextButton.interactable = false;
            if (LevelLoader.instance.nextButtonClickedValue + 2 > PlayerPrefs.GetInt("levelAt"))
            {
                Debug.Log("s");
                PlayerPrefs.SetInt("levelAt", LevelLoader.instance.nextButtonClickedValue + 2);
            }
            SceneManager.LoadScene(1);

        }
        else if(LevelLoader.instance.nextButtonClickedValue == maxLevels)
        {
            Debug.Log("Game Win");
            nextButton.interactable = false;
            winPanel.SetActive(false);
            geniusPanel.SetActive(true);

        }
        
    }
    public void RestartButtonClick()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
