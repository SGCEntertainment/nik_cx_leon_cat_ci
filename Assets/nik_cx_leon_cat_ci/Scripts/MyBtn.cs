using UnityEngine;
using UnityEngine.EventSystems;

public class MyBtn : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Windows windows;

    public void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.Open(windows);
    }
}
