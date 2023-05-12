using System.Collections;
using TMPro;
using UnityEngine;

public class CrystalFloatingText : MonoBehaviour
{
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private float _endHeight = 0.0f;
    [SerializeField] private float _duration = 5.0f;
    [SerializeField] private float _randomRange = 1.5f;
    [SerializeField] private AnimationCurve _transitionCurve = new AnimationCurve();
    [SerializeField] private TextMeshProUGUI _textOne = null;
    [SerializeField] private TextMeshProUGUI _textTwo = null;
    [SerializeField] private TextMeshProUGUI _textThree = null;
    private Camera _camera = null;
    private int _crystalStage = 1;
    private bool _isActive = false;

    public void Activate()
    {
        _crystalStage = 1;
        _isActive = true;
    }

    public void Initialize()
    {
        _camera = ServiceLocator.Get<CameraUtil>().CameraCO;

        _textOne.gameObject.SetActive(false);
        _textTwo.gameObject.SetActive(false);
        _textThree.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isActive == false)
        {
            return;
        }

        BillBoard();
    }

    private void BillBoard()
    {
        transform.LookAt(_camera.transform);
        transform.Rotate(0, 180, 0);
    }

    public void Click(int amount)
    {
        TextMeshProUGUI textToMove = GetText();
        if (textToMove == null)
        {
            return;
        }

        textToMove.text = $"+{amount} <sprite name=Essence>";
        ++_crystalStage;

        StartCoroutine(MoveUpFadeCoroutine(textToMove));
    }

    private TextMeshProUGUI GetText()
    {
        if (_crystalStage == 1)
        {
            return _textOne;
        }
        else if (_crystalStage == 2)
        {
            return _textTwo;
        }
        else if (_crystalStage == 3)
        {
            return _textThree;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator MoveUpFadeCoroutine(TextMeshProUGUI text)
    {
        float timer = 0.0f;

        Color finalColor = _textColor;
        Color startingColor = finalColor;
        startingColor.a = 0.0f;

        float rand = Random.Range(-_randomRange, _randomRange);

        Vector3 startPos = transform.position;
        startPos.x +=rand;

        Vector3 endPos = startPos;
        endPos.y += _endHeight;

        text.gameObject.SetActive(true);
        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float linearProgress = timer / _duration;

            float easeProgress = _transitionCurve.Evaluate(linearProgress);
            text.transform.position = Vector3.Lerp(startPos, endPos, easeProgress);

            if (linearProgress < 0.5f)
            {
                text.color = Color.Lerp(startingColor, finalColor, linearProgress);
            }
            else
            {
                text.color = Color.Lerp(finalColor, startingColor, linearProgress);
            }

            yield return null;
        }
        text.gameObject.SetActive(false);

        if (text == _textThree)
        {
            _isActive = false;
        }
    }
}