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
        var newChimera = CreateChimeraByType(chimeraInfo.ChimeraType);

        LoadChimeraStats(newChimera.GetComponent<Chimera>(), chimeraInfo);

        return newChimera;
    }

    public GameObject CreateChimeraByType(ChimeraType chimeraType)
    {
        var chimeraGO = Instantiate(_resourceManager.GetChimeraBasePrefab(chimeraType));
        Instantiate(_resourceManager.GetChimeraEvolution(chimeraType), chimeraGO.transform);

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
