using UnityEngine;

public class ChimeraCreator : MonoBehaviour
{
    private ResourceManager _resourceManager = null;

    public ChimeraCreator Initialize()
    {
        Debug.Log($"<color=Cyan> Initializing {this.GetType()} ... </color>");

        _resourceManager = ServiceLocator.Get<ResourceManager>();

        return this;
    }

    public GameObject CreateChimera(ChimeraType chimeraType)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(chimeraType);
        var chimeraEvolutionGO = _resourceManager.GetChimeraEvolution(chimeraType);

        chimeraEvolutionGO.transform.parent = chimeraGO.transform;

        return chimeraGO;
    }
}
