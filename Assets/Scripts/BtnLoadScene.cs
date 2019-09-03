using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class BtnLoadScene : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneUtil.LoadScene(name);
    }
}
