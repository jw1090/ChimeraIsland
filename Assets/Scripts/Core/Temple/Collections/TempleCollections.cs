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
    private Collections _chimeraCollections = null;

    public GameObject A{ get => _a; }
    public GameObject A1 { get => _a1; }
    public GameObject A2 { get => _a2; }
    public GameObject A3 { get => _a3; }
    public GameObject B { get => _b; }
    public GameObject B1 { get => _b1; }
    public GameObject B2 { get => _b2; }
    public GameObject B3 { get => _b3; }
    public GameObject C { get => _c; }
    public GameObject C1 { get => _c1; }
    public GameObject C2 { get => _c2; }
    public GameObject C3 { get => _c3; }

    public void Initialize()
    {
        _chimeraCollections = ServiceLocator.Get<HabitatManager>().Collections;
        _a.GetComponent<Outline>().enabled = false;
        _a1.GetComponent<Outline>().enabled = false;
        _a2.GetComponent<Outline>().enabled = false;
        _a3.GetComponent<Outline>().enabled = false;
        _b.GetComponent<Outline>().enabled = false;
        _b1.GetComponent<Outline>().enabled = false;
        _b2.GetComponent<Outline>().enabled = false;
        _b3.GetComponent<Outline>().enabled = false;
        _c.GetComponent<Outline>().enabled = false;
        _c1.GetComponent<Outline>().enabled = false;
        _c2.GetComponent<Outline>().enabled = false;
        _c3.GetComponent<Outline>().enabled = false;
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
