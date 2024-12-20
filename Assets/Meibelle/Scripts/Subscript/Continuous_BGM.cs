using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continuous_BGM : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
        GameObject[] levels_BGM = GameObject.FindGameObjectsWithTag("Levels_BGM");

        Debug.Log(BGM.Length);
        if (BGM.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (levels_BGM.Length == 1)
            {
                Destroy(BGM[0]);
            }
            else if (levels_BGM.Length == 0)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
