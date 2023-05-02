using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
    [SerializeField] private float _minDistance = 0.1f;

    private Color _pathColor;
    private LineRenderer _lineRenderer;
    private int _positionNumber = 0;

    public int PointNumber => _lineRenderer.positionCount;

    private void Awake()
    {
        _pathColor = GetComponent<Player>().Color;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startColor = _pathColor;
        _lineRenderer.endColor = _pathColor;
    }

    private void OnEnable()
    {
        SetPoint();
    }

    private void Update()
    {
        DrawLine();
    }

    public void DeleteAllPoint()
    {
        _positionNumber = 0;
        _lineRenderer.positionCount = _positionNumber;
    }

    public Vector2 GetPointPosition(int index)
    {
        return _lineRenderer.GetPosition(index);
    }

    private void DrawLine()
    {
        if (_minDistance <= Vector3.Distance((MousePosition()), (_lineRenderer.GetPosition(_positionNumber - 1))))
            SetPoint();
    }

    private void SetPoint()
    {
        _lineRenderer.positionCount = _positionNumber + 1;
        _lineRenderer.SetPosition(_positionNumber++, MousePosition());
    }

    private Vector3 MousePosition()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward);
    }
}
