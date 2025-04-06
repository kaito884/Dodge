using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;
    private Timer timer;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        StartCoroutine(EnableTrap());
    }

    IEnumerator EnableTrap()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            traps[i].SetActive(true);
            yield return new WaitForSeconds(timer.timeLimit / traps.Length);
        }
    }
}
