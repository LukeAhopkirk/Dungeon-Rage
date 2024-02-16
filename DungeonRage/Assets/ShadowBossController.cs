using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBossController : MonoBehaviour
{
    public GameObject textPanel;
    // Start is called before the first frame update
    void Start()
    {
        textPanel.SetActive(false);
    }

    // Update is called once per frame
    public void textPanelActive()
    {
        textPanel.SetActive(true);
    }
}
