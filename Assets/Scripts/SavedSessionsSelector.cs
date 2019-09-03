using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavedSessionsSelector : MonoBehaviour
{
    private int m_selectedSessionIndex = 0;
    private List<GameSessionRecord> m_sessions;
    [SerializeField] Text m_textSessionName, m_textLastLogin;
    [SerializeField] Button[] m_buttonsToDisableWhenNoSession;
    private void Start()
    {
        m_sessions = GameSessionRecord.EnumerateSavedSessions();
        //Debug.Log(m_sessions.Count);
        //Debug.Assert(m_sessions.Count > 0);
        if (m_sessions.Count == 0)
        {
            m_textSessionName.text = "No Sessions found";
            m_textLastLogin.enabled = false;
            foreach (var btn in m_buttonsToDisableWhenNoSession)
            {
                btn.interactable = false;
            }
        }
        PrintSelectedSession();
    }
    private void Update()
    {
        //PrintSelectedSession();
    }
    private void PrintSelectedSession()
    {
        if (m_sessions.Count == 0) return;
        var selectedSession = m_sessions[m_selectedSessionIndex];
        selectedSession.TryGetGameRecord<string[]>("@Players", out var users);
        m_textSessionName.text = $"{users[0]} and {users[1]}";
        selectedSession.TryGetGameRecord<DateTime>("@LastLogin", out var lastLogin);
        m_textLastLogin.text = lastLogin.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
    }
    public void GoToPrevious()
    {
        if (m_selectedSessionIndex > 0) --m_selectedSessionIndex;
        PrintSelectedSession();
    }
    public void GoToNext()
    {
        if (m_selectedSessionIndex < m_sessions.Count - 1) ++m_selectedSessionIndex;
        PrintSelectedSession();
    }
    public void Confirm()
    {
        GameSession.Instance.Init(m_sessions[m_selectedSessionIndex]);
        SceneUtil.LoadScene("FallingDown");
    }
}
