using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;

public class gameplay : MonoBehaviour
{
    // Start is called before the first frame update
    FlowerSystem fs;
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("default");
        fs.SetupDialog();
        fs.ReadTextFromResource("dialogue_intro");
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
