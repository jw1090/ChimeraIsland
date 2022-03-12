using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    //private List<GameObject> effect;
    private NavMeshAgent agent;
    private List<Facility> facilitiesList;
    private IdleState idleState;

    public FacilitiesState facilitiesState;
    public int index;
    public bool isAi;
    public float time;
    public string like;
    public string normal;
    public string dislike;
    public float stayTime;

    void OnEnable()
    {
       
        idleState = GetComponent<IdleState>();
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
        }
        */
        if (facilitiesState.transformsFacilities.Count == 0)
            return;
        agent.isStopped = false;
        agent.SetDestination(facilitiesState.transformsFacilities[index].position);
    }
    public void Update()
    {
        //Debug.Log(Vector3.Distance(this.transform.position, facilitiesList[index].transform.position));
        if (facilitiesState.transformsFacilities.Count == 0)
            return;
            if (Vector3.Distance(this.transform.position, facilitiesState.transformsFacilities[index].position) < 5)
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
                }
                */
                if ((time += Time.deltaTime) > stayTime)
                {
                    ++index;

                    if (index == facilitiesState.transformsFacilities.Count)
                    {
                        /*
                        effect[0].SetActive(false);
                        effect[1].SetActive(false);
                        effect[2].SetActive(false);
                        */
                        index = 0;

                        idleState.enabled = true;
                        enabled = false;
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
