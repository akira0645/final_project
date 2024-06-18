using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.SceneManagement;
using System;

public class FinalSceneController : MonoBehaviour
{
    FlowerSystem fs;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            fs = FlowerManager.Instance.GetFlowerSystem("default");
        }
        catch (Exception e)
        {
            fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
        }
        fs.SetupDialog();
        fs.ReadTextFromResource("Ending");
        fs.RegisterCommand("load_scene", (List<string> _params) => {
            SceneManager.LoadScene(_params[0]);
        });
        /*fs.RegisterCommand("hide_score_board", (List<string> _params) => {
            Canvas c = GameObject.FindAnyObjectByType<Canvas>();
            c.hideFlags = HideFlags.HideInInspector;
        });*/
        fs.RegisterCommand("lock_attcak", (List<string> _params) => {
            GameObject g = GameObject.Find("player");
            g.GetComponent<player_controller>().canAttack = false;
        });
        fs.RegisterCommand("release_attcak", (List<string> _params) => {
            GameObject g = GameObject.Find("player");
            g.GetComponent<player_controller>().canAttack = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            fs.Next();
        }
    }
}