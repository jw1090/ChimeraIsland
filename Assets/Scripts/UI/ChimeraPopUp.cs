using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChimeraPopUp : MonoBehaviour
{
    private Camera _camera = null;
    private Chimera _chimera = null;
    private bool _initialized = false;
    private ResourceManager _resourceManager = null;
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private Image _type = null;
    [SerializeField] private TextMeshProUGUI _exploration = null;
    [SerializeField] private TextMeshProUGUI _stamina = null;
    [SerializeField] private TextMeshProUGUI _wisdom = null;

    public void Initialize()
    {
        _camera = ServiceLocator.Get<CameraUtil>().CameraCO;
        _resourceManager = ServiceLocator.Get<ResourceManager>();
        _initialized = true; 
    }

    public void SetChimera(Chimera chimera)
    {
        _chimera = chimera;
        if(chimera == null)
        {
            return;
        }
        _name.text = chimera.Name;
        _type.sprite = _resourceManager.GetElementDetailsSprite(_chimera.ElementalType);
        _exploration.text = chimera.Exploration.ToString();
        _stamina.text = chimera.Stamina.ToString();
        _wisdom.text = chimera.Wisdom.ToString();
    }

    private void Update()
    {
        if (_initialized == false || gameObject.activeInHierarchy == false || _chimera == null)
        {
            return;
        }
        transform.position = _chimera.transform.position + new Vector3(-4.0f,4.0f,0.0f);
        transform.LookAt(_camera.transform);
    }
}
