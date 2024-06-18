using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transsport : MonoBehaviour
{
    // Start is called before the first frame update
    public int pointNumber;
    void Start()
    {
        if(this.transform.tag!="Knight")
            this.transform.tag = "portal";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeScene(string a)
    {

        SceneManager.LoadScene(a);
        CheckInit.startPointNmber = pointNumber;
        //SceneManager.LoadScene("BossIntro");
    }
}
