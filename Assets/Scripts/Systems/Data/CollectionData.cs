using System;

[Serializable]
public class CollectionData
{ 
    public bool A = false;
    public bool A1 = false;
    public bool A2 = false;
    public bool A3 = false;
    public bool B = false;
    public bool B1 = false;
    public bool B2 = false;
    public bool B3 = false;
    public bool C = false;
    public bool C1 = false;
    public bool C2 = false;
    public bool C3 = false;

    public CollectionData(ChimeraCollections chimeraCollections)
    {
        A = chimeraCollections.AUnlocked;
        A1 = chimeraCollections.A1Unlocked;
        A2 = chimeraCollections.A2Unlocked;
        A3 = chimeraCollections.A3Unlocked;
        B = chimeraCollections.BUnlocked;
        B1 = chimeraCollections.B1Unlocked;
        B2 = chimeraCollections.B2Unlocked;
        B3 = chimeraCollections.B3Unlocked;
        C = chimeraCollections.CUnlocked;
        C1 = chimeraCollections.C1Unlocked;
        C2 = chimeraCollections.C2Unlocked;
        C3 = chimeraCollections.C3Unlocked;
    }

    public CollectionData() { }
}