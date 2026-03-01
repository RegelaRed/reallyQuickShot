using UnityEngine;
using UnityEngine.Events;

public class CanInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private IInteract[] actions;

    public UnityEvent unityEvent;
    Outline outline;
    public void Awake()
    {
        actions = GetComponents<IInteract>();

        outline = gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false;
    }
    public void SetHighlight(bool values = false)
    {
        outline.enabled = values;
    }
    public void Call()
    {
        unityEvent.Invoke();
    }
}
