using System.Collections;
using UnityEngine;
using TMPro;

public class TwoProcessPeterson : MonoBehaviour
{
    public static int turn;
    public static bool[] flags = new bool[2];

    [SerializeField] private float executionTime = 5f;
    [SerializeField] private Process[] processes;
    [SerializeField] private Transform criticalRegion;
    [SerializeField] private Transform waitRegion;
    [SerializeField] private TMP_Text turnText;

    void OnDisable()
    {
        ResetAll();
    }

    public void _QueueProcesses()
    {
        for (int i = 0; i < processes.Length; i++)
        {
            StartCoroutine(TwoProcAlgorithm(i));
        }
    }

    public IEnumerator TwoProcAlgorithm(int pID)
    {
        print("p" + pID + " chegou");
        int other = 1 - pID;

        flags[pID] = true;
        processes[pID].RaiseFlag();
        turn = other;

        yield return new WaitForSeconds(2f);

        while (flags[other] == true && turn == other)
        {
            //Espera
            processes[pID].transform.position = waitRegion.position;
            yield return null;
        }

        //Se��o Cr�tica
        StartCoroutine(CriticalSection(pID));
        

    }

    IEnumerator CriticalSection(int pID)
    {
        processes[pID].transform.position = criticalRegion.position;
        processes[pID].EnableTimer();

        yield return new WaitForSeconds(executionTime);

        flags[pID] = false;
        processes[pID].LowerFlag();
        processes[pID].ResetPosition();
    }


    void ResetAll()
    {
        for (int i = 0; i < processes.Length; i++)
        {
            processes[i].ResetPosition();
            flags[i] = false;
        }
    }

}
