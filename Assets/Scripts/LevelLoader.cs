using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    
    public GameObject[] levels;
    public int nextButtonClickedValue;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        nextButtonClickedValue = PlayerPrefs.GetInt("NextButtonClicked", 0);
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(false);
        }
        switch (nextButtonClickedValue)
        {
            case 0:
                FirstValueAssign(3, 0);
                break;
            case 1:
                FirstValueAssign(5, 1);
                break;
            case 2:
                FirstValueAssign(2, 2);
                break;
            case 3:
                FirstValueAssign(3, 3);
                break;
            case 4:
                FirstValueAssign(9, 4);
                break;
            case 5:
                FirstValueAssign(2, 0);
                break;
            case 6:
                FirstValueAssign(5, 1);
                break;
            case 7:
                FirstValueAssign(4, 2);
                break;
            case 8:
                FirstValueAssign(1, 3);
                break;
            case 9:
                FirstValueAssign(2, 4);
                break;
        }
        levels[nextButtonClickedValue].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FirstValueAssign(int value, int colorValue)
    {
        PlayerController.instance.firstValue = value;
        PlayerController.instance.valueText.text = value.ToString();
        PlayerController.instance.playerMaterial.color = PlayerController.instance.differentColors[colorValue];
    }
}
