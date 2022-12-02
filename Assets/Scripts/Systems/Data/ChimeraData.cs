using System;

[Serializable]
public class ChimeraData
{
    public ChimeraType Type = ChimeraType.None;
    public int Exploration = 0;
    public int Stamina = 0;
    public int Wisdom = 0;
    public int CurrentEnergy = 0;
    public int UniqueId = 0;
    public bool First = false;
    public string CustomName = null;

    public ChimeraData(Chimera chimera)
    {
        First = chimera.FirstChimera;
        Type = chimera.ChimeraType;
        Stamina = chimera.Stamina;
        Wisdom = chimera.Wisdom;
        Exploration = chimera.Exploration;
        CurrentEnergy = chimera.CurrentEnergy;
        CustomName = chimera.CustomName;
        UniqueId = chimera.UniqueID;
    }
}