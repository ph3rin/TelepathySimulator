using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnBackToMainMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneUtil.LoadScene("MainMenu");
    }
}
