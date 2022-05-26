using System;

//will add more later
[Serializable]
public class GlobalData
{
    public int currentEssence = 0;
    public GlobalData(int essence)
    {
        currentEssence = essence;
    }
}
