using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Light _light = null;
    [SerializeField] private float _lightRange = 3.0f;
    [SerializeField] private float _lightIntensity = 28.0f;

    public void Grow(int currentTier)
    {
        _animator.enabled = true;
        _animator.SetInteger("Grow", currentTier);
    }

    public void ResetCrystal()
    {
        _light.range = _lightRange;
        _light.intensity = _lightIntensity;
        _animator.enabled = false;
    }
}