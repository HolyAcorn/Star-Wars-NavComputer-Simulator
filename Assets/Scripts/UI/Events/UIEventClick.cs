using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace SwNavComp
{
    public class UIEventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] UnityEvent leftClickEvent;
        [SerializeField] UnityEvent rightClickEvent;
        [SerializeField] UnityEvent pointerDownEvent;
        [SerializeField] UnityEvent pointerUpEvent;
        [SerializeField] UnityEvent pointerEnterEvent;
        [SerializeField] UnityEvent pointerExitEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) rightClickEvent.Invoke();
            else if (eventData.button == PointerEventData.InputButton.Left) leftClickEvent.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDownEvent.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            pointerEnterEvent.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            pointerExitEvent.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pointerUpEvent.Invoke();
        }
    }
}
