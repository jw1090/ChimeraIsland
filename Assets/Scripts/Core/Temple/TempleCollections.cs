using UnityEngine;

public class TempleCollections : MonoBehaviour
{
    [Header("Chimeras")]
    [SerializeField] private GameObject _a = null;
    [SerializeField] private GameObject _a1 = null;
    [SerializeField] private GameObject _a2 = null;
    [SerializeField] private GameObject _a3 = null;
    [SerializeField] private GameObject _b = null;
    [SerializeField] private GameObject _b1 = null;
    [SerializeField] private GameObject _b2 = null;
    [SerializeField] private GameObject _b3 = null;
    [SerializeField] private GameObject _c = null;
    [SerializeField] private GameObject _c1 = null;
    [SerializeField] private GameObject _c2 = null;
    [SerializeField] private GameObject _c3 = null;
    private ChimeraCollections _chimeraCollections = null;

    public void Initialize()
    {
        _chimeraCollections = ServiceLocator.Get<HabitatManager>().ChimeraCollections;
    }

    public void Build()
    {
        _a.SetActive(_chimeraCollections.AUnlocked);
        _a1.SetActive(_chimeraCollections.A1Unlocked);
        _a2.SetActive(_chimeraCollections.A2Unlocked);
        _a3.SetActive(_chimeraCollections.A3Unlocked);

        _b.SetActive(_chimeraCollections.BUnlocked);
        _b1.SetActive(_chimeraCollections.B1Unlocked);
        _b2.SetActive(_chimeraCollections.B2Unlocked);
        _b3.SetActive(_chimeraCollections.B3Unlocked);

        _c.SetActive(_chimeraCollections.CUnlocked);
        _c1.SetActive(_chimeraCollections.C1Unlocked);
        _c2.SetActive(_chimeraCollections.C2Unlocked);
        _c3.SetActive(_chimeraCollections.C3Unlocked);
    }
}
