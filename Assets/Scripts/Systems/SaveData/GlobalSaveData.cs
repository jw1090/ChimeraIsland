using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//will add more later
[Serializable]
public class GlobalSaveData
{
    public int currentEssence = 0;
    public GlobalSaveData(int essence)
    {
        currentEssence = essence;
    }
}
