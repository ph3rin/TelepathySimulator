using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __GenTest : MonoBehaviour
{
    private int level = 0;
    private int cur_gen_depth_ = 0;
    private FallingDownLevelGenerator gen;
    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        gen = new FallingDownLevelGenerator();
        //StartCoroutine(CrtGen());
    }

    IEnumerator CrtGen()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            //gen.GenerateLevel(level);
            ++level;
        }
        
    }

    private void Update()
    {
        //transform.position -= new Vector3(0f, 30f * Time.deltaTime);
        var gen_to_dpeth = Mathf.FloorToInt(Mathf.Abs(transform.position.y) / 3f) + 10;
        //int count = 0;
        while (cur_gen_depth_ < gen_to_dpeth)
        {
            var level = gen.GenerateLevel();
            level.transform.position = new Vector3(0f, -this.cur_gen_depth_ * 3f, 0f);
            ++cur_gen_depth_;
            //++count;
            level.SetActive(true);
            //Debug.Log(cur_gen_depth_);
        }
        //print(count);
    }
}