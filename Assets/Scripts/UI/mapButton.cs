using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapButton : MonoBehaviour
{
    public Animator transition;
    [SerializeField] GameObject mapUI;
    [SerializeField] GameObject gameUI;
    public float transitionTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void onClickPlains()
    {
        transition.SetBool("Start", true);
        StartCoroutine(transitionP());
    }
    IEnumerator transitionP()
    {
        yield return new WaitForSeconds(transitionTime);
        mapUI.SetActive(false);
        gameUI.SetActive(true);
        transition.SetBool("Start", false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
