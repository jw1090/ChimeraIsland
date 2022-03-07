using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //private List<GameObject> effect;
    private NavMeshAgent agent;
    private List<Facility> facilitiesList;
    private IdleMoveAI idleMoveAI;

    public int index;
    public bool isAi;
    public float time;
    public string like;
    public string normal;
    public string dislike;
    public float stayTime;

    private void Start()
    {
        facilitiesList = GameManager.Instance.GetActiveHabitat().GetFacilities();
        idleMoveAI = GetComponent<IdleMoveAI>();
        agent = this.GetComponent<NavMeshAgent>();

        time = 0;
        index = 0;
        SetPos(index);
        /*
        for (int i = 0; i < effect.Count; i++)
        {
            effect[i].SetActive(false);
        }*/
    }

    public void SetPos(int index) {
        /*
        for (int i = 0; i < effect.Count; i++)
        {
            effect[i].SetActive(false);
        }*/
        agent.isStopped = false;
        agent.SetDestination(facilitiesList[index].transform.position);
    }
    public void Update()
    {
        //Debug.Log(Vector3.Distance(this.transform.position, facilitiesList[index].transform.position));

        if (Vector3.Distance(this.transform.position, facilitiesList[index].transform.position) < 4)
        {
           // RemoveVerticalVelocity();
           // Going.text = "arrive" + TargetTransForm[indexpos].gameObject.name;
            agent.isStopped = true;

            /*
            // Effects
            if (like.Contains(index.ToString()))
            {

                effect[0].SetActive(true);
                effect[1].SetActive(false);
                effect[2].SetActive(false);

            }
            if (dislike.Contains(index.ToString()))
            {
                effect[0].SetActive(false);
                effect[1].SetActive(true);
                effect[2].SetActive(false);
            }
            if (normal.Contains(index.ToString()))
            {
                effect[0].SetActive(false);
                effect[1].SetActive(false);
                effect[2].SetActive(true);
            }*/


            if ((time += Time.deltaTime) > stayTime)
            {
                ++index;

                if (index == facilitiesList.Count)
                {
                    /* Cleanup
                    effect[0].SetActive(false);
                    effect[1].SetActive(false);
                    effect[2].SetActive(false);
                    */
                    //index = 0;

                    idleMoveAI.enabled = true;
                    idleMoveAI.ResetIndex();
                    Debug.Log("Moving to idleMotionState - Idle Component Status: " + idleMoveAI.enabled);

                    enabled = false;
                    Debug.Log("Last Patrol nodes have been reached! - Player Controller Status: " + enabled);
                }
                else
                {
                    time = 0;
                    SetPos(index);
                }
            }
        }
    }
}
