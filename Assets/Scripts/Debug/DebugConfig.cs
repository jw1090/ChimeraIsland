using System.Collections;
using UnityEngine;

public class DebugConfig : MonoBehaviour, IGameModule
{
    [SerializeField] private bool _enableTutorials = true;
    public bool TutorialsEnabled { get { return _enableTutorials; } }

    public IEnumerator LoadModule()
    {
        Debug.Log("Loading DebugConfig Module");
        ServiceLocator.Register<DebugConfig>(this);
        yield return null;
    }
}
