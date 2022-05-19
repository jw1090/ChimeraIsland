using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    private ChimeraCreator _chimeraCreator = null;
    public ChimeraCreator ChimeraCreator { get => _chimeraCreator; }

    public ToolsManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        InitializeToolsSystems();

        return this;
    }

    private void InitializeToolsSystems()
    {
        Debug.Log("Loading Tools");

        var chimeraCreatorGO = new GameObject("Chimera Creator");
        chimeraCreatorGO.transform.SetParent(transform);
        _chimeraCreator = chimeraCreatorGO.AddComponent<ChimeraCreator>().Initialize();

        Debug.Log("Tools Finished Initializing");
    }
}
