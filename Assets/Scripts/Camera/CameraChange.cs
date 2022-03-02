using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraChange : MonoBehaviour
{

    private GameObject Camera_0;

    private GameObject Camera_1;

    void Start()
    {

        Camera_0 = GameObject.Find("Island Camera");

        Camera_1 = GameObject.Find("Follow Camera");

        Camera_1.SetActive(false);

    }

    

    public void OnClick()

    {

        

    }
}

    