using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Weapon _weapon;

    private SpriteRenderer _spriteRenderer;
    private bool _isTurnRight;
    private int _curretnHealth;

    public bool IsTurnRight => _isTurnRight;

    private void Start()
    {
        _isTurnRight = true;
        _curretnHealth = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ApplyDamage(int damage)
    {
        _curretnHealth -= damage;
        _curretnHealth = Mathf.Clamp(_curretnHealth, 0, _maxHealth);
    }

    public void ApplyHeal(int heal)
    {
        _curretnHealth += heal;
        _curretnHealth = Mathf.Clamp(_curretnHealth, 0, _maxHealth);
    }

    public void TurnLeft()
    {
        _isTurnRight = false;
        _spriteRenderer.flipX = true;
    }

    public void TurnRight()
    {
        _isTurnRight = true;
        _spriteRenderer.flipX = false;
    }

    public void SetPosition(float positionX,float positionY)
    {
        transform.position = new Vector3(positionX, positionY, transform.position.z);
    }
}