using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Hero8DirMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 50;

    private Vector2 _movement;
    private float x;
    private float y;

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
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        _movement.x = x;
        _movement.y = y;

        _animator.SetInteger("Horizontal", (int)x);
        _animator.SetInteger("Vertical", (int)y);
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(
            _rigidbody2D.position +
            _movement *
            _moveSpeed *
            Time.fixedDeltaTime);
    }

}
