using System.Collections;
using UnityEngine;

public class MonoUtil : MonoBehaviour
{
    public MonoUtil Initialize()
    {
        return this;
    }

    public void StartCoroutineEx(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}