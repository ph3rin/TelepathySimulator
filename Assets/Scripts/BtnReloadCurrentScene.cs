using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnReloadCurrentScene : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        SceneUtil.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        if (FallingDownSession.Instance.State == FallingDownGameState.Ended)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                ReloadCurrentScene();
        }
    }
}
