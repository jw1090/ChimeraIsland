using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerContruller : MonoBehaviour
{
    public List<GameObject> Effect;
    private NavMeshAgent agent;
    public List<Transform> TargetTransForm;
    public int index;
    public bool isAi;
    public float time;
    public string like;
    public string normal;
    public string dislike;
    public float stayTime;
    // Start is called before the first frame update
    void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();
        time = 0;
        index = 0;
        SetPos(index);
        for (int i = 0; i < Effect.Count; i++)
        {
            Effect[i].SetActive(false);
        }
    }

    public void SetPos(int index) {
        for (int i = 0; i < Effect.Count; i++)
        {
            Effect[i].SetActive(false);
        }
        agent.isStopped = false;
        agent.SetDestination(TargetTransForm[index].position);
    }
    public void Update()
    {
        print(Vector3.Distance(this.transform.position, TargetTransForm[index].position));
        if (Vector3.Distance(this.transform.position, TargetTransForm[index].position) < 4)
        {
           // RemoveVerticalVelocity();
           // Going.text = "arrive" + TargetTransForm[indexpos].gameObject.name;
            agent.isStopped = true;
            //Play
            if (like.Contains(index.ToString()))
            {
                Effect[0].SetActive(true);
                Effect[1].SetActive(false);
                Effect[2].SetActive(false);

            }
            if (dislike.Contains(index.ToString()))
            {
                Effect[0].SetActive(false);
                Effect[1].SetActive(true);
                Effect[2].SetActive(false);
            }
            if (normal.Contains(index.ToString()))
            {
                Effect[0].SetActive(false);
                Effect[1].SetActive(false);
                Effect[2].SetActive(true);
            }
            if ((time+=Time.deltaTime)> stayTime)
            {
              
                index ++;
                if (index==TargetTransForm.Count)
                {
                    //关闭所有表情
                    Effect[0].SetActive(false);
                    Effect[1].SetActive(false);
                    Effect[2].SetActive(false);
                    //index = 0;
                    //已经走到最后一位置点。开启巡逻模式
                    this.GetComponent<idleMoveAI>().enabled = true;
                    this.enabled = false;
                    print("到达最后一个点转到待机巡逻");
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
