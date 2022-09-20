using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class HeroTranslateAstar : HeroTranslate
{
    [SerializeField]
    private float nextWayPointDistance = 0.1f;

    private Seeker _seeker;
    private Rigidbody2D _rb2D;

    private Path path;
    private int currentWayPoint = 0;
    protected override void OnStart()
    {
        _seeker = GetComponent<Seeker>();
        _rb2D = GetComponent<Rigidbody2D>();
        AstarPath.active.Scan();
    }
    protected override void SetPath(Vector3 target)
    {
        _seeker.StartPath(_rb2D.position, target, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (p.error is false)
        {
            path = p;
            currentWayPoint = 0;
            StopCoroutine(MoveByPath());
            StartCoroutine(MoveByPath());
        }
    }

    IEnumerator MoveByPath()
    {
        while (true)
        {
            if (path is not null)
            {
                if (currentWayPoint >= path.vectorPath.Count)
                {
                    Move(0, 0);
                    EndMove();
                    yield break;
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - _rb2D.position).normalized;
                Move(direction.x, direction.y);

                float distance = Vector2.Distance(_rb2D.position, path.vectorPath[currentWayPoint]);

                if (distance < nextWayPointDistance)
                {
                    currentWayPoint++;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
