using UnityEngine;

public class Redirect : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject interactable;

    public void OnInteract()
    {
        var interact  = interactable.GetComponent<IInteractable>();
        if (interact is not null)
        {
            interact.OnInteract();
        }
    }
}
