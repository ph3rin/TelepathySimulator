using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingDownDisplayDepth : MonoBehaviour
{
    private Text m_text;
    private void Awake()
    {
        m_text = GetComponent<Text>();
    }
    private void Update()
    {
        if (FallingDownSession.Instance?.State == FallingDownGameState.InGame)
        {
            m_text.text = $"{FallingDownSession.Instance.GetMaxPlayerDepth()}M";
        }
        
    }
}
