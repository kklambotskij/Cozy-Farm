using UnityEngine;
using UnityEngine.Events;

public class HeroTranslate : HeroController
{
    protected virtual void SetPath(Vector3 target) { }
    protected void EndMove() 
    {
        if (OnEndMove is not null)
        {
            OnEndMove.Invoke();
        }
    }

    private UnityAction OnEndMove;

    public void SetMoveTarget(Vector3 vector3)
    {
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
            SetPath((Vector2)hit.transform.position);
        }
        else
        {
            SetPath(vector3);
        }
    }
}
