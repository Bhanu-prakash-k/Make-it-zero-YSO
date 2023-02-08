using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionCanvas : MonoBehaviour
{
    public Animator fingerAnimation;

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position == new Vector3(1, 0, 0) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            fingerAnimation.SetBool("SlideDown", true);
        }
        if(player.position == new Vector3(1,0,0) && SceneManager.GetActiveScene().buildIndex == 2)
        {
            fingerAnimation.SetBool("SlideUp", true);
        }
    }
}
