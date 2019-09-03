using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class TimeUtil : IDisposable
{
    private static TimeUtil s_instance;
    public static TimeUtil Instance => s_instance;
    private IPauseEventSource m_pause_event_source;
    private float m_pause_time, m_start_time, m_pause_start_time;
    private bool m_is_paused = false;
    public TimeUtil(IPauseEventSource pause_event_source)
    {
        m_start_time = UnityEngine.Time.time;
        m_pause_event_source = pause_event_source;
        pause_event_source.OnPause += () =>
        {
            m_pause_start_time = UnityEngine.Time.time;
            m_is_paused = true;
        };
        pause_event_source.OnUnpause += () =>
        {
            m_pause_time += UnityEngine.Time.time - m_pause_start_time;
            m_is_paused = false;
        };
        s_instance = this;
    }
    public float Time
    {
        get
        {
            return (m_is_paused ? m_pause_start_time :
                    UnityEngine.Time.time) - m_start_time - m_pause_time;
        }
    }

    public void Dispose()
    {
        s_instance = null;
    }
}