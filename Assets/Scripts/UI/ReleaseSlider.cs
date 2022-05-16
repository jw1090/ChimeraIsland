using UnityEngine;
using UnityEngine.UI;

public class ReleaseSlider : MonoBehaviour
{
    [SerializeField] private float _heldSeconds = 2.0f;

    private Slider _slider = null;

    private float _heldCounter = 0.0f;
    public float HeldCounter { get => _heldCounter; }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void Hold(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Facility") && hit.collider.GetComponent<Facility>().IsChimeraStored())
        {
            _heldCounter += Time.deltaTime;
            if (_heldCounter >= _heldSeconds)
            {
                hit.collider.GetComponent<Facility>().RemoveChimera();
                ResetSlider();
            }
        }
    }

    public void ResetSlider()
    {
        _heldCounter = 0.0f;
    }

    public void UpdateSliderUI()
    {
        // Control remove slider
        if (_heldCounter > 0.0f)
        {
            gameObject.SetActive(true);
            transform.position = Input.mousePosition + new Vector3(75.0f, 0.0f, 0.0f);
            _slider.value = _heldCounter / 2.0f;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
