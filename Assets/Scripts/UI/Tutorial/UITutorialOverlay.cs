using UnityEngine;

public class UITutorialOverlay : MonoBehaviour
{
    [SerializeField] private TextInfo _textInfo = null;
    private ResourceManager _resourceManager = null;
    private int stepNumber = 0;
    private bool stop = false;

    [System.Serializable]
    public class GlobalTutorial
    {
        Tutorials[] Tutorials;
    }

    public void Initialize()
    {
        _resourceManager = ServiceLocator.Get<ResourceManager>();
    }

    public void NextStep()
    {
        if(stop == true)
        {
            this.gameObject.SetActive(false);
            Debug.Log("You have reached the total steps");
        }
        stepNumber++;
        ShowOverlay();
 
    }

    public void ShowOverlay()
    {
        GlobalTutorial tutorialData = FileHandler.ReadFromJSON<GlobalTutorial>(GameConsts.JsonSaveKeys.TUTORIAL_DATA_FILE);
        if (tutorialData == null)
        {
            Debug.Log($"No tutorial Data found");
            //tutorialData = new DialogSteps();
        }

        //if(stepNumber < tutorialData.Steps.Length)
        //{ 
        //    DialogInfo loadedStep = tutorialData.Steps[stepNumber];
        //    Sprite icon = _resourceManager.GetChimeraSprite(loadedStep.type);

        //    Debug.Log($"Current Tutorial Step: { stepNumber}");
        //    Debug.Log($"Descrpition: { loadedStep.description }  Icon: { loadedStep.type }");
        //    _textInfo.Load(tutorialData.Steps[stepNumber].description, icon);
        //}
        //else
        //{
        //    stop = true;
        //}
    }
}