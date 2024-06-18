using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemove : StateMachineBehaviour
{
    public float fadeTime = 0.5f;
    private float timeElapsed = 0f;
    GameObject objToRemove;
    Transsport transsport;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        objToRemove=animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(objToRemove.name);
        if (objToRemove.name =="knight")
        {
            //changeScene
            objToRemove.transform.GetComponent<Transsport>().ChangeScene("FinalScene");
        }
            timeElapsed += Time.deltaTime;
        if (timeElapsed > fadeTime)
        {
            Destroy(objToRemove);
        }
    }
}
