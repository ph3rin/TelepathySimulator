using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class PlayerInputDefault : IPlayerInput
{
    private KeyCode m_left, m_right, m_up;
    public PlayerInputDefault(KeyCode left, KeyCode right, KeyCode up)
    {
        m_left = left;
        m_right = right;
        m_up = up;
    }
    public bool GetLeftKey()
    {
        return Input.GetKey(m_left);
    }

    public bool GetLeftKeyDown()
    {
        return Input.GetKeyDown(m_left);
    }

    public bool GetRightKey()
    {
        return Input.GetKey(m_right);
    }

    public bool GetRightKeyDown()
    {
        return Input.GetKeyDown(m_right);
    }

    public bool GetUpKey()
    {
        return Input.GetKey(m_up);
    }

    public bool GetUpKeyDown()
    {
        return Input.GetKeyDown(m_up);
    }
}