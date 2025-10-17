using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const String CUT = "Cut";
    private Animator animator;
    [SerializeField] private CuttingCounter cuttingCounter;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
