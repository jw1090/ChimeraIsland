using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    public ToolsManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        InitializeToolsSystems();

        return this;
    }

    private void InitializeToolsSystems()
    {
        var chimeraCreatorGO = new GameObject("Chimera Creator");
        chimeraCreatorGO.transform.SetParent(transform);
        var chimeraCreatorComp = chimeraCreatorGO.AddComponent<ChimeraCreator>().Initialize();
        ServiceLocator.Register<ChimeraCreator>(chimeraCreatorComp);

        var sceneChangerGO = new GameObject("Scene Changer");
        sceneChangerGO.transform.SetParent(transform);
        var sceneChangerComp = sceneChangerGO.AddComponent<SceneChanger>().Initialize();
        ServiceLocator.Register<SceneChanger>(sceneChangerComp);

        Debug.Log("Tools Finished Initializing");
    }
}