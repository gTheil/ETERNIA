using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    public AudioSource buttonHover;
    public AudioSource buttonClick;

    public virtual void OnSelect(BaseEventData eventData) { 
        buttonHover.Play();
    }

    public virtual void OnSubmit(BaseEventData eventData) { 
        buttonClick.Play();
    }
}
