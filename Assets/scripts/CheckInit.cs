using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckInit : MonoBehaviour
{
    public GameObject playerobj;
    public static string DebugScene;
    public static int startPointNmber;
    // Start is called before the first frame update
    void Start()
    {
        playerobj = GameObject.Find("player");
        if (!playerobj)
        {
            SceneManager.LoadScene("game");
            DebugScene = SceneManager.GetActiveScene().name;
        }
        if (startPointNmber != 0)
        {
            GameObject g = GameObject.Find(startPointNmber.ToString()) as GameObject;
            if (g != null)
            {
                print("suffhdhs " + startPointNmber.ToString());
                playerobj.transform.position = g.transform.position;
            }
            startPointNmber = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
