using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class FallingDownScanLine : MonoBehaviour
{
    [SerializeField] float m_amplitude, m_period, m_minSpeed;
    public float speed;
    private void Update()
    {
        if (FallingDownSession.Instance.State == FallingDownGameState.InGame)
        {
            //TODO: implement elapsed time
            speed = -Mathf.Cos(2 * Mathf.PI / m_period * Time.time) * m_amplitude + m_amplitude + m_minSpeed + 
                FallingDownSession.Instance.Difficulty;
            var cam = Camera.main;
            var v = cam.ScreenToWorldPoint(new Vector3(Screen.width * (cam.rect.xMin + cam.rect.xMax) / 2.0f, Screen.height * cam.rect.yMax, z: cam.nearClipPlane));
            if (v.y < transform.position.y)
            {
                transform.position = v;
            }
            else
            {
                transform.Translate(0, (float)(-speed * (double)Time.deltaTime), 0);
            }
        }
    }
}
