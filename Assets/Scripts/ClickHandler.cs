using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private Vector3 target;

    public UnityEvent<Vector3> OnClick = new UnityEvent<Vector3>();

    public static ClickHandler Instance;

    private void Awake()
    {
        if (Instance is not null)
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
}
