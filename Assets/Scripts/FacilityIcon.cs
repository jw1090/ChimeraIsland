using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    void Start()
    {
        icon.sprite = null;
    }
    private void Update()
    {
        this.transform.LookAt(Camera.main.transform);
    }
    public void setIcon(Sprite s)
    {
        icon.sprite = s;
    }
    public void removeIcon()
    {
        icon.sprite = null;
    }
    
}
