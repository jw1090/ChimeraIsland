using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private Sprite _defaultChimeraSprite = null;
    private Sprite _chimeraASprite = null;
    private Sprite _chimeraA1Sprite = null;
    private Sprite _chimeraA2Sprite = null;
    private Sprite _chimeraA3Sprite = null;
    private Sprite _chimeraBSprite = null;
    private Sprite _chimeraB1Sprite = null;
    private Sprite _chimeraB2Sprite = null;
    private Sprite _chimeraB3Sprite = null;
    private Sprite _chimeraCSprite = null;
    private Sprite _chimeraC1Sprite = null;
    private Sprite _chimeraC2Sprite = null;
    private Sprite _chimeraC3Sprite = null;

    private Sprite _aquaSprite = null;
    private Sprite _bioSprite = null;
    private Sprite _pyraSprite = null;

    private Sprite _tutorialCute = null;
    private Sprite _tutorialHappy = null;
    private Sprite _tutorialLookingLeft = null;
    private Sprite _tutorialLookingRight = null;
    private Sprite _tutorialShocked = null;
    private Sprite _tutorialSpeaking = null;
    private Sprite _tutorialStandard = null;

    private GameObject _chimeraBasePrefabA = null;
    private GameObject _chimeraBasePrefabB = null;
    private GameObject _chimeraBasePrefabC = null;
    private GameObject _chimeraEvolutionPrefabA = null;
    private GameObject _chimeraEvolutionPrefabA1 = null;
    private GameObject _chimeraEvolutionPrefabA2 = null;
    private GameObject _chimeraEvolutionPrefabA3 = null;
    private GameObject _chimeraEvolutionPrefabB = null;
    private GameObject _chimeraEvolutionPrefabB1 = null;
    private GameObject _chimeraEvolutionPrefabB2 = null;
    private GameObject _chimeraEvolutionPrefabB3 = null;
    private GameObject _chimeraEvolutionPrefabC = null;
    private GameObject _chimeraEvolutionPrefabC1 = null;
    private GameObject _chimeraEvolutionPrefabC2 = null;
    private GameObject _chimeraEvolutionPrefabC3 = null;
    private GameObject _uiManagerPrefab = null;

    public GameObject UIManager { get => _uiManagerPrefab;}

    public ResourceManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        _defaultChimeraSprite = Resources.Load<Sprite>("Icons/Chimera/DefaultChimera-Icon");
        _chimeraASprite = Resources.Load<Sprite>("Icons/Chimera/A-Icon");
        _chimeraA1Sprite = Resources.Load<Sprite>("Icons/Chimera/A1-Icon");
        _chimeraA2Sprite = Resources.Load<Sprite>("Icons/Chimera/A2-Icon");
        _chimeraA3Sprite = Resources.Load<Sprite>("Icons/Chimera/A3-Icon");
        _chimeraBSprite = Resources.Load<Sprite>("Icons/Chimera/B-Icon");
        _chimeraB1Sprite = Resources.Load<Sprite>("Icons/Chimera/B1-Icon");
        _chimeraB2Sprite = Resources.Load<Sprite>("Icons/Chimera/B2-Icon");
        _chimeraB3Sprite = Resources.Load<Sprite>("Icons/Chimera/B3-Icon");
        _chimeraCSprite = Resources.Load<Sprite>("Icons/Chimera/C-Icon");
        _chimeraC1Sprite = Resources.Load<Sprite>("Icons/Chimera/C1-Icon");
        _chimeraC2Sprite = Resources.Load<Sprite>("Icons/Chimera/C2-Icon");
        _chimeraC3Sprite = Resources.Load<Sprite>("Icons/Chimera/C3-Icon");

        _pyraSprite = Resources.Load<Sprite>("Icons/Chimera/C1-Icon");
        _aquaSprite = Resources.Load<Sprite>("Icons/Chimera/C3-Icon");
        _bioSprite = Resources.Load<Sprite>("Icons/Chimera/C2-Icon");

        _tutorialCute = Resources.Load<Sprite>("Icons/Tutorial/Cute");
        _tutorialHappy = Resources.Load<Sprite>("Icons/Tutorial/Happy");
        _tutorialLookingLeft = Resources.Load<Sprite>("Icons/Tutorial/Looking Left");
        _tutorialLookingRight = Resources.Load<Sprite>("Icons/Tutorial/Looking Right");
        _tutorialShocked = Resources.Load<Sprite>("Icons/Tutorial/Shocked");
        _tutorialSpeaking = Resources.Load<Sprite>("Icons/Tutorial/Speaking");
        _tutorialStandard = Resources.Load<Sprite>("Icons/Tutorial/Standard");

        _chimeraBasePrefabA = Resources.Load<GameObject>("Chimera/A");
        _chimeraBasePrefabB = Resources.Load<GameObject>("Chimera/B");
        _chimeraBasePrefabC = Resources.Load<GameObject>("Chimera/C");
        _chimeraEvolutionPrefabA = Resources.Load<GameObject>("Chimera/Models/Family A/A Model");
        _chimeraEvolutionPrefabA1 = Resources.Load<GameObject>("Chimera/Models/Family A/A1 Model");
        _chimeraEvolutionPrefabA2 = Resources.Load<GameObject>("Chimera/Models/Family A/A2 Model");
        _chimeraEvolutionPrefabA3 = Resources.Load<GameObject>("Chimera/Models/Family A/A3 Model");
        _chimeraEvolutionPrefabB = Resources.Load<GameObject>("Chimera/Models/Family B/B Model");
        _chimeraEvolutionPrefabB1 = Resources.Load<GameObject>("Chimera/Models/Family B/B1 Model");
        _chimeraEvolutionPrefabB2 = Resources.Load<GameObject>("Chimera/Models/Family B/B2 Model");
        _chimeraEvolutionPrefabB3 = Resources.Load<GameObject>("Chimera/Models/Family B/B3 Model");
        _chimeraEvolutionPrefabC = Resources.Load<GameObject>("Chimera/Models/Family C/C Model");
        _chimeraEvolutionPrefabC1 = Resources.Load<GameObject>("Chimera/Models/Family C/C1 Model");
        _chimeraEvolutionPrefabC2 = Resources.Load<GameObject>("Chimera/Models/Family C/C2 Model");
        _chimeraEvolutionPrefabC3 = Resources.Load<GameObject>("Chimera/Models/Family C/C3 Model");

        _uiManagerPrefab = Resources.Load<GameObject>("UI Manager");

        return this;
    }

    public GameObject GetChimeraBasePrefab(ChimeraType chimeraType)
    {
        switch (chimeraType)
        {
            case ChimeraType.A:
            case ChimeraType.A1:
            case ChimeraType.A2:
            case ChimeraType.A3:
                return _chimeraBasePrefabA;
            case ChimeraType.B:
            case ChimeraType.B1:
            case ChimeraType.B2:
            case ChimeraType.B3:
                return _chimeraBasePrefabB;
            case ChimeraType.C:
            case ChimeraType.C1:
            case ChimeraType.C2:
            case ChimeraType.C3:
                return _chimeraBasePrefabC;
            default:
                Debug.LogError($"Unhandled chimera type: {chimeraType}. Please change!");
                return null;
        }
    }

    public GameObject GetChimeraEvolution(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraEvolutionPrefabA;
            case ChimeraType.A1:
                return _chimeraEvolutionPrefabA1;
            case ChimeraType.A2:
                return _chimeraEvolutionPrefabA2;
            case ChimeraType.A3:
                return _chimeraEvolutionPrefabA3;
            case ChimeraType.B:
                return _chimeraEvolutionPrefabB;
            case ChimeraType.B1:
                return _chimeraEvolutionPrefabB1;
            case ChimeraType.B2:
                return _chimeraEvolutionPrefabB2;
            case ChimeraType.B3:
                return _chimeraEvolutionPrefabB3;
            case ChimeraType.C:
                return _chimeraEvolutionPrefabC;
            case ChimeraType.C1:
                return _chimeraEvolutionPrefabC1;
            case ChimeraType.C2:
                return _chimeraEvolutionPrefabC2;
            case ChimeraType.C3:
                return _chimeraEvolutionPrefabC3;
            default:
                Debug.LogError($"Unhandled chimera type: {type}");
                return null;
        }
    }

    public Sprite GetTutorialSprite(TutorialIconType type)
    {
        switch (type)
        {
            case TutorialIconType.Cute:
                return _tutorialCute;
            case TutorialIconType.Happy:
                return _tutorialHappy;
            case TutorialIconType.LookingLeft:
                return _tutorialLookingLeft;
            case TutorialIconType.LookingRight:
                return _tutorialLookingRight;
            case TutorialIconType.Shocked:
                return _tutorialShocked;
            case TutorialIconType.Speaking:
                return _tutorialSpeaking;
            case TutorialIconType.Standard:
                return _tutorialStandard;
            default:
                Debug.LogError($"Unhandled sprite type: {type}");
                return null;
        }
    }

    public Sprite GetChimeraSprite(ChimeraType type)
    {
        switch (type)
        {
            case ChimeraType.A:
                return _chimeraASprite;
            case ChimeraType.A1:
                return _chimeraA1Sprite;
            case ChimeraType.A2:
                return _chimeraA2Sprite;
            case ChimeraType.A3:
                return _chimeraA3Sprite;
            case ChimeraType.B:
                return _chimeraBSprite;
            case ChimeraType.B1:
                return _chimeraB1Sprite;
            case ChimeraType.B2:
                return _chimeraB2Sprite;
            case ChimeraType.B3:
                return _chimeraB3Sprite;
            case ChimeraType.C:
                return _chimeraCSprite;
            case ChimeraType.C1:
                return _chimeraC1Sprite;
            case ChimeraType.C2:
                return _chimeraC2Sprite;
            case ChimeraType.C3:
                return _chimeraC3Sprite;
            default:
                Debug.LogWarning($"Returning Default Sprite, please change.");
                return _defaultChimeraSprite;
        }
    }
}