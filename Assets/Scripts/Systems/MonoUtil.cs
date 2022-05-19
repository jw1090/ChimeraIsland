using System.Collections;
using UnityEngine;

public class MonoUtil : MonoBehaviour
{
    public MonoUtil Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} Initialized!</color>");

        return this;
    }

    public void StartCoroutineEx(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}