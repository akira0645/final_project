using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    FlowerSystem fs;
    // Start is called before the first frame update
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("lk", false);
        fs.SetupDialog();
        fs.ReadTextFromResource("Intro");
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