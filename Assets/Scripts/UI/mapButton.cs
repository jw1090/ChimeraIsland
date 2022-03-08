using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapButton : MonoBehaviour
{
    public Animator transition;
    [SerializeField] GameObject mapUI;
    [SerializeField] GameObject crossFade;
    public float transitionTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(crossfadeOff());
    }
    public void onClickPlains()
    {
        crossFade.SetActive(true);
        transition.SetBool("start", true);
        StartCoroutine(transitionP());
    }
    IEnumerator transitionP()
    {
        yield return new WaitForSeconds(transitionTime);
        this.GetComponent<Image>().enabled = false;
        mapUI.SetActive(false);
        transition.SetBool("start", false); 
        yield return new WaitForSeconds(transitionTime);
        crossFade.SetActive(false);
    }
    IEnumerator crossfadeOff()
    {
        yield return new WaitForSeconds(transitionTime);
        crossFade.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
