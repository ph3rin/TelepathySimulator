using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal enum SceneTransitionState
{
    Idle, Filling, Fading
}

internal class SceneUtil : MonoBehaviour
{
    private static SceneUtil Instance { get; set; }
    [SerializeField] private float m_fadeTime;
    private SceneTransitionState m_state;
    private string m_sceneName;
    [SerializeField] private RawImage m_image;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        switch (m_state)
        {
            case SceneTransitionState.Idle:
                {
                    break;
                }
            case SceneTransitionState.Filling:
                {
                    var fadeSpeed = 1.0f / m_fadeTime;
                    var color = m_image.color;
                    if (color.a >= 1.0f)
                    {
                        m_state = SceneTransitionState.Fading;
                        SceneManager.LoadScene(m_sceneName);
                        return;
                    }
                    color.a += Time.deltaTime * fadeSpeed;
                    m_image.color = color;
                    break;
                }
            case SceneTransitionState.Fading:
                {
                    var fadeSpeed = 1.0f / m_fadeTime;
                    var color = m_image.color;
                    if (color.a <= 0.0f)
                    {
                        Instance.GetComponent<GraphicRaycaster>().enabled = false;
                        m_state = SceneTransitionState.Idle;
                        return;
                    }
                    color.a -= Time.deltaTime * fadeSpeed;
                    m_image.color = color;
                    break;
                }
        }
    }
    public bool IsSceneInteractable()
    {
        return Instance.m_state == SceneTransitionState.Idle;
    }
    public static void LoadScene(string name)
    {
        if (Instance == null)
        {
            SceneManager.LoadScene(name);
            return;
        }
        Instance.m_sceneName = name;
        Instance.GetComponent<GraphicRaycaster>().enabled = true;
        Debug.Assert(Instance.m_state != SceneTransitionState.Filling);
        Instance.m_state = SceneTransitionState.Filling;
    }
}
