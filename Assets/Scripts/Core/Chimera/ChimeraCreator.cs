using UnityEngine;

public class ChimeraCreator : MonoBehaviour
{
    private ResourceManager _resourceManager = null;

    public ChimeraCreator Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _resourceManager = ServiceLocator.Get<ResourceManager>();

        return this;
    }

    public GameObject CreateChimera(ChimeraData chimeraInfo)
    {
        var newChimera = CreateChimeraByType(chimeraInfo.chimeraType);

        LoadChimeraStats(newChimera.GetComponent<Chimera>(), chimeraInfo);

        return newChimera;
    }

    public GameObject CreateChimeraByType(ChimeraType chimeraType)
    {
        var chimeraGO = Instantiate(_resourceManager.GetChimeraBasePrefab(chimeraType));
        Instantiate(_resourceManager.GetChimeraEvolution(chimeraType), chimeraGO.transform);

        return chimeraGO;
    }

    private void LoadChimeraStats(Chimera newChimera, ChimeraData chimeraInfo)
    {
        newChimera.SetLevel(chimeraInfo.level);
        newChimera.SetEndurance(chimeraInfo.endurance);
        newChimera.SetIntelligence(chimeraInfo.intelligence);
        newChimera.SetStrength(chimeraInfo.strength);
    }
}