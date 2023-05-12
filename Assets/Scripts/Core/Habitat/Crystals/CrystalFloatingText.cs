using System.Collections;
using TMPro;
using UnityEngine;

public class CrystalFloatingText : MonoBehaviour
{
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private float _endHeight = 0.0f;
    [SerializeField] private float _duration = 5.0f;
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

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position;
        endPos.y += _endHeight;

        text.gameObject.SetActive(true);
        while (timer < _duration)
        {
            timer += Time.deltaTime;
            float progress = timer / _duration;

            text.transform.position = Vector3.Lerp(startPos, endPos, progress);


            if (progress < 0.5f)
            {
                text.color = Color.Lerp(startingColor, finalColor, progress);
            }
            else
            {
                text.color = Color.Lerp(finalColor, startingColor, progress);
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