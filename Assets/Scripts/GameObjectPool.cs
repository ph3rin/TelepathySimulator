using System;
using System.Collections.Concurrent;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameObjectPool
{
    private ConcurrentQueue<GameObject> object_queue_;
    private GameObject object_prototype_;
    public GameObjectPool(GameObject prototype)
    {
        object_queue_ = new ConcurrentQueue<GameObject>();
        object_prototype_ = prototype;
    }
    public GameObject Claim()
    {
        GameObject gm = null;
        if (!object_queue_.TryDequeue(out gm))
        {
            gm = GameObject.Instantiate(object_prototype_, Vector3.zero, Quaternion.identity);
        }
        gm.SetActive(true);
        return gm;
    }

    public void Recycle(GameObject gm)
    {
        if (!gm.activeInHierarchy) return;
        if (gm == null) return;
        Reset(gm);
        gm.SetActive(false);
        object_queue_.Enqueue(gm);
    }

    private void Reset(GameObject gm)
    {
        if (gm.GetComponent<Rigidbody2D>() != null)
        {
            Object.Destroy(gm.GetComponent<Rigidbody2D>());
        }
        gm.transform.position = Vector3.zero;
        gm.transform.rotation = Quaternion.identity;
        gm.transform.SetParent(null);
        gm.GetComponent<SpriteRenderer>().color = Color.white;
    }
}