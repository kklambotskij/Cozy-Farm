using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class HeroController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 50;

    [SerializeField]
    private float _timeToIdle = 3;

    private Vector2 _movement;
    private bool _isIdleTimerStart;
    private bool _isIdle;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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

        if (Mathf.Abs(_movement.x) >= Mathf.Abs(_movement.y))
        {
            _animator.SetInteger("Horizontal", System.Math.Sign(x));
            _animator.SetInteger("Vertical", 0);
        }
        else
        {
            _animator.SetInteger("Horizontal", 0);
            _animator.SetInteger("Vertical", System.Math.Sign(y));
        }
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
}
