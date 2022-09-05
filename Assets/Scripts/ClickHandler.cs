using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private Vector3 target;

    public UnityEvent<Vector3> OnClick = new UnityEvent<Vector3>();

    public static ClickHandler Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnClick.Invoke(target);
        }
    }

    private void SetAction()
    {
        RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero);
        if (hit.collider != null)
        {
            print(hit.collider.name);
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnInteract();
            }
        }
    }
}
