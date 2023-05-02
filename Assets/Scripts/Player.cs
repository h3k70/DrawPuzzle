using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(LineDrawer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _id = 0;

    private Color _color;
    private LineDrawer _lineDrawer;
    private Vector3[] _path;
    private bool _isReady = false;
    private Coroutine _mvingToPointJob;

    public bool IsReady => _isReady;
    public Color Color => _color;

    public event UnityAction Dying;
    public event UnityAction PathCreated;
    public event UnityAction PointHasBeenReached;

    private void OnValidate()
    {
        if (_id == 0)
            _color= Color.blue;
        else if (_id == 1)
            _color = Color.red;
    }

    private void Start()
    {
        _lineDrawer = GetComponent<LineDrawer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && _lineDrawer.enabled == true)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<EndPoint>(out EndPoint endPoint))
                {
                    if (endPoint.Id == _id || endPoint.Id == -1)
                    {
                        CreatePath();
                    }
                }
            }
            TryDeleteLine();
            _lineDrawer.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player) || collision.TryGetComponent<Wall>(out Wall wall))
        {
            Dying?.Invoke();
            StopCoroutine(_mvingToPointJob);
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (_isReady == false)
            _lineDrawer.enabled = true;
    }

    public void StartMoving(float duration)
    {
        Tween tween = transform.DOPath(_path, duration, PathType.Linear).SetEase(Ease.Linear);
        _mvingToPointJob = StartCoroutine(MovingToPoint(duration));
    }

    private void CreatePath()
    {
        _path = new Vector3[_lineDrawer.PointNumber];

        for(int i = 0; i < _lineDrawer.PointNumber; i++)
        {
            _path[i] = _lineDrawer.GetPointPosition(i);
        }
        _isReady = true;
        PathCreated?.Invoke();
    }

    private void TryDeleteLine()
    {
        if (_isReady != true)
            _lineDrawer.DeleteAllPoint();
    }

    private IEnumerator MovingToPoint(float duration)
    {
        yield return new WaitForSeconds(duration);
        PointHasBeenReached?.Invoke();
        yield break;
    }
}
