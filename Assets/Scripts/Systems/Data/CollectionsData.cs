using System;

[Serializable]
public class CollectionsData
{ 
    public bool a = false;
    public bool a1 = false;
    public bool a2 = false;
    public bool a3 = false;
    public bool b = false;
    public bool b1 = false;
    public bool b2 = false;
    public bool b3 = false;
    public bool c = false;
    public bool c1 = false;
    public bool c2 = false;
    public bool c3 = false;

    public CollectionsData(ChimeraCollections chimeraCollections)
    {
        a = chimeraCollections.A1Unlocked;
        a1 = chimeraCollections.A1Unlocked;
        a2 = chimeraCollections.A2Unlocked;
        a3 = chimeraCollections.A3Unlocked;
        b = chimeraCollections.BUnlocked;
        b1 = chimeraCollections.B1Unlocked;
        b2 = chimeraCollections.B2Unlocked;
        b3 = chimeraCollections.B3Unlocked;
        c = chimeraCollections.CUnlocked;
        c1 = chimeraCollections.C1Unlocked;
        c2 = chimeraCollections.C2Unlocked;
        c3 = chimeraCollections.C3Unlocked;
    }
}