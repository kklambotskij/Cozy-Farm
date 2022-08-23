using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Hero8DirMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 50;

    [SerializeField]
    private float _timeToIdle = 3;

    private Vector2 _movement;
    private int x;
    private int y;

    private float _idleTimer;
    private bool _isIdleTimerStart;
    private bool _isIdle;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        x = (int)Input.GetAxisRaw("Horizontal");
        y = (int)Input.GetAxisRaw("Vertical");

        if (x == 0 && y == 0)
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

        _animator.SetInteger("Horizontal", x);
        _animator.SetInteger("Vertical", y);
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
