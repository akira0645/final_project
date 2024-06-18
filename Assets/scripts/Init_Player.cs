using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init_Player : MonoBehaviour
{
    public string start;
    // Start is called before the first frame update
    void Start()
    {
        if(CheckInit.DebugScene==null)
        {
            SceneManager.LoadScene(start);
        }
        else
        {
            SceneManager.LoadScene(CheckInit.DebugScene);
            CheckInit.DebugScene = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
