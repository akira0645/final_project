using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer=1.5f;
    public float speed;
    void Start()
    {
        //this.GetComponent<Rigidbody>().AddForce(new Vector2(20, 0),ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameObject.GetComponent<SpriteRenderer>().flipX)
        {
            this.gameObject.transform.position += new Vector3(speed * Time.deltaTime * 60, 0, 0);
        }
        else
        {
            this.gameObject.transform.position -= new Vector3(speed * Time.deltaTime * 60, 0, 0);
        }
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
