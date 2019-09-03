using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnLogoutAndBackToMainMenu : MonoBehaviour
{
    public void OnClick()
    {
        GameSession.Instance?.Logout();
        SceneUtil.LoadScene("MainMenu");
    }
}
