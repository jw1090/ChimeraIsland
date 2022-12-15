public class Collections
{
    private bool _aUnlocked = false;
    private bool _a1Unlocked = false;
    private bool _a2Unlocked = false;
    private bool _a3Unlocked = false;
    private bool _bUnlocked = false;
    private bool _b1Unlocked = false;
    private bool _b2Unlocked = false;
    private bool _b3Unlocked = false;
    private bool _cUnlocked = false;
    private bool _c1Unlocked = false;
    private bool _c2Unlocked = false;
    private bool _c3Unlocked = false;

    public bool AUnlocked { get => _aUnlocked; }
    public bool A1Unlocked { get => _a1Unlocked; }
    public bool A2Unlocked { get => _a2Unlocked; }
    public bool A3Unlocked { get => _a3Unlocked; }
    public bool BUnlocked { get => _bUnlocked; }
    public bool B1Unlocked { get => _b1Unlocked; }
    public bool B2Unlocked { get => _b2Unlocked; }
    public bool B3Unlocked { get => _b3Unlocked; }
    public bool CUnlocked { get => _cUnlocked; }
    public bool C1Unlocked { get => _c1Unlocked; }
    public bool C2Unlocked { get => _c2Unlocked; }
    public bool C3Unlocked { get => _c3Unlocked; }

    public void LoadData (CollectionData collectionData)
    {
        _aUnlocked = collectionData.A;
        _a1Unlocked = collectionData.A1;
        _a2Unlocked = collectionData.A2;
        _a3Unlocked = collectionData.A3;
        _bUnlocked = collectionData.B;
        _b1Unlocked = collectionData.B1;
        _b2Unlocked = collectionData.B2;
        _b3Unlocked = collectionData.B3;
        _cUnlocked= collectionData.C;
        _c1Unlocked = collectionData.C1;
        _c2Unlocked = collectionData.C2;
        _c3Unlocked = collectionData.C3;
    }

    public void CollectChimera(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
                _aUnlocked = true;
                break;
            case ChimeraType.A1:
                _a1Unlocked = true;
                break;
            case ChimeraType.A2:
                _a2Unlocked = true;
                break;
            case ChimeraType.A3:
                _a3Unlocked = true;
                break;
            case ChimeraType.B:
                _bUnlocked = true;
                break;
            case ChimeraType.B1:
                _b1Unlocked = true;
                break;
            case ChimeraType.B2:
                _b2Unlocked = true;
                break;
            case ChimeraType.B3:
                _b3Unlocked = true;
                break;
            case ChimeraType.C:
                _cUnlocked = true;
                break;
            case ChimeraType.C1:
                _c1Unlocked = true;
                break;
            case ChimeraType.C2:
                _c2Unlocked = true;
                break;
            case ChimeraType.C3:
                _c3Unlocked = true;
                break;
            default:
                break;
        }
    }
}