using System.Collections;
using UnityEngine;

public class MonoUtil : MonoBehaviour
{
    public MonoUtil Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        return this;
    }

    public void StartCoroutineEx(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}