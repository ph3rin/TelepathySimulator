using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class NewSessionBtnConfirm : MonoBehaviour
{
    [SerializeField] private Text m_errorText;
    [SerializeField] private InputField m_user1, m_user2;
    [SerializeField] private GameSession m_sessionObject;
    public void Confirm()
    {
        Regex validator = new Regex(@"^[a-zA-Z0-9][A-Za-z0-9_-]*$");
        if (!validator.IsMatch(m_user1.text) || !validator.IsMatch(m_user2.text))
        {
            m_errorText.text = "Error: Username may not be empty and may " +
                "only contain letters, numbers and underscores.";
        }
        else if (SessionAlreadyExists())
        {
            m_errorText.text = "Error: Session already exists.";
        }
        else
        {
            m_sessionObject.Init(GameSessionRecord.CreateNew(m_user1.text, m_user2.text));
            Debug.Assert(m_sessionObject == GameSession.Instance);
            DontDestroyOnLoad(m_sessionObject.gameObject);
            SceneUtil.LoadScene("FallingDown");
        }
    }
    private bool SessionAlreadyExists()
    {
        var path = $"{Application.persistentDataPath}/{m_user1.text.ToLower()}-{m_user2.text.ToLower()}.json";
        return (File.Exists(path));
    }
}
