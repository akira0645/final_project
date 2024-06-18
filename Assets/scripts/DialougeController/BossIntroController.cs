using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.SceneManagement;
using System;

public class BossIntro : MonoBehaviour
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
        fs.ReadTextFromResource("boss_battle_intro");
        fs.RegisterCommand("load_scene", (List<string> _params) => {
            SceneManager.LoadScene(_params[0]);
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