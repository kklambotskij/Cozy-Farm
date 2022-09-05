using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 50;

    [SerializeField]
    private float _timeToIdle = 3;

    //Test Variables
    [SerializeField]
    private Vector3 target;

    [SerializeField]
    private float distance;

    public UnityAction OnEndMove;

    private Vector2 _movement;
    private bool _isIdleTimerStart;
    private bool _isIdle;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

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

    public void Move(float x, float y)
    {
        if (x is 0 && y is 0)
        {
            if (!_isIdle && !_isIdleTimerStart)
            {
                _isIdleTimerStart = true;
                StartCoroutine(WaitForIdle());
            }
        }
        else
        {
            _isIdle = false;
            _isIdleTimerStart = false;
        }

        _movement.x = x;
        _movement.y = y;

        _animator.SetInteger("Horizontal", (int)x);
        _animator.SetInteger("Vertical", (int)y);
        _animator.SetBool("Idle", _isIdle);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(
            _rigidbody2D.position +
            _movement *
            _moveSpeed *
            Time.fixedDeltaTime);
    }

    private IEnumerator WaitForIdle()
    {
        yield return new WaitForSeconds(_timeToIdle);
        _isIdle = true;
    }

    private IEnumerator MoveHeroIgnoringObstacles()
    {
        Vector3 whereToMove = (target - transform.position).normalized;
        while (Vector2.Distance(transform.position, target) > 0.01)
        {
            distance = Vector2.Distance(transform.position, target);
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
