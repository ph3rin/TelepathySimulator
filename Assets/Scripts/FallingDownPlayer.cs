using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
internal class FallingDownPlayer : MonoBehaviour
{
    private string m_name;
    [SerializeField]
    private float m_baseSpeed, m_baseGravityScale, m_baseSpeedMultiplier, m_logarithmVerticalScaler, m_logarithmHorizontalScaler;
    private float m_growth = 1.0f;
    private float m_speedMultiplier = 1.0f;
    private Rigidbody2D m_rigidBody;
    private CircleCollider2D m_collider;
    private IPlayerInput m_playerInput;
    private Dictionary<string, float> m_modifiers = new Dictionary<string, float>();
    public float Speed;
    private Action m_unpauseAction;
    private void Awake()
    {
        gameObject.tag = "Player";
        m_rigidBody = this.GetComponent<Rigidbody2D>();
        m_collider = this.GetComponent<CircleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Is it another player?
        if (collision.gameObject.CompareTag("Player"))
        {
            FallingDownSession.Instance.EndGame();
        }
    }
    public void Init(IPlayerInput playerInput)
    {
        m_playerInput = playerInput;
        FallingDownSession.Instance.OnPause += () =>
        {
            var velocity = m_rigidBody.velocity;
            m_rigidBody.isKinematic = true;
            m_unpauseAction = () =>
            {
                m_rigidBody.velocity = velocity;
            };
        };
        FallingDownSession.Instance.OnUnpause += () =>
        {
            m_rigidBody.isKinematic = false;
            m_unpauseAction();
        };
    }
    private void FixedUpdate()
    {
        if (FallingDownSession.Instance.State == FallingDownGameState.InGame)
        {
            Speed = GetMaxVelocity();
            var v = m_rigidBody.velocity;
            v.x = GetVelocity();
            if (Mathf.Abs(v.y) > 0.5f) v.x *= 0.5f;
            m_rigidBody.velocity = v;
            m_rigidBody.gravityScale = GetGravityScale();
        }
        else
        {
            m_rigidBody.velocity = Vector2.zero;
        }
    }
    private float GetVelocity()
    {
        Debug.Assert(m_playerInput != null);
        var direction = 0f;
        if (m_playerInput.GetLeftKey())
        {
            direction = -1f;
        }
        else if (m_playerInput.GetRightKey())
        {
            direction = 1f;
        }
        return direction * GetMaxVelocity();
    }
    public void AddModifier(string name, float expireIn)
    {
        if (!m_modifiers.ContainsKey(name))
        {
            m_modifiers.Add(name, TimeUtil.Instance.Time + expireIn);
        }
        else
        {
            m_modifiers[name] = Mathf.Max(m_modifiers[name], TimeUtil.Instance.Time + expireIn);
        }
    }
    public bool HasModifer(string name)
    {
        return m_modifiers.TryGetValue(name, out var expire) && expire >= TimeUtil.Instance.Time;
    }
    public void Grow(float growth)
    {
        m_growth += growth;
    }
    private float GetSpeedGrowth()
    {
        return m_logarithmVerticalScaler * Mathf.Log(m_logarithmHorizontalScaler * m_growth + 1);
    }
    private float GetMaxVelocity()
    {
        var multiplier = m_baseSpeedMultiplier;
        if (HasModifer("Acceleration"))
            multiplier *= 1.4f;
        return (m_baseSpeed + GetSpeedGrowth()) * multiplier;
    }
    private float GetGravityScale()
    {
        return m_baseGravityScale * GetMaxVelocity() / m_baseSpeed;
    }
}