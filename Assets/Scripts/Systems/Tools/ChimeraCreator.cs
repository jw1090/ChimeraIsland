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

    public GameObject CreateChimera(Chimera chimeraInfo)
    {
        var chimeraGO = _resourceManager.GetChimeraBasePrefab(chimeraInfo.Type);
        var chimeraEvolutionGO = _resourceManager.GetChimeraEvolution(chimeraInfo.Type);

        chimeraEvolutionGO.transform.parent = chimeraGO.transform;

        LoadChimeraStats(chimeraGO.GetComponent<Chimera>(), chimeraInfo);

        return chimeraGO;
    }

    private void LoadChimeraStats(Chimera newChimera, Chimera chimeraInfo)
    {
        newChimera.Level        = chimeraInfo.Level;
        newChimera.Endurance    = chimeraInfo.Endurance;
        newChimera.Intelligence = chimeraInfo.Intelligence;
        newChimera.Strength     = chimeraInfo.Strength;
        newChimera.Happiness    = chimeraInfo.Happiness;
    }
}
