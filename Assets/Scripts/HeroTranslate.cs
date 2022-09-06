using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HeroTranslate : HeroController
{
    [SerializeField]
    private Vector3 target;

    private UnityAction OnEndMove;

    protected new void Start()
    {
        base.Start();
        ClickHandler.Instance.OnClick.AddListener(SetMoveTarget);
    }

    public void SetMoveTarget(Vector3 vector3)
    {
        target = vector3;
        StopCoroutine(MoveHeroIgnoringObstacles());
        StartCoroutine(MoveHeroIgnoringObstacles());

        OnEndMove = null;

        RaycastHit2D hit = Physics2D.Raycast(vector3, Vector2.zero);
        if (hit.collider is not null)
        {
            print(hit.collider.name);
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();
            if (interactable is not null)
            {
                OnEndMove = () => interactable.OnInteract();
            }
        }
    }

    private IEnumerator MoveHeroIgnoringObstacles()
    {
        Vector3 whereToMove = (target - transform.position).normalized;
        while (Vector2.Distance(transform.position, target) > 0.01)
        {
            Move(whereToMove.x, whereToMove.y);
            yield return new WaitForFixedUpdate();
        }
        Move(0, 0);
        if (OnEndMove is not null)
        {
            OnEndMove.Invoke();
        }
    }
}
