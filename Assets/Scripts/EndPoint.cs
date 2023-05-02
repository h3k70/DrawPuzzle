using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EndPoint : MonoBehaviour
{
    [SerializeField] private int _id = 0;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public int Id => _id;

    private void OnValidate()
    {
        if (_id == 0)
            _spriteRenderer.color = Color.blue;
        else if (_id == 1)
            _spriteRenderer.color = Color.red;
        else
            _spriteRenderer.color = new Color(1, 0, 1, 1);
    }
}
