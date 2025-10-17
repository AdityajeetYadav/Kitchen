using System;
using UnityEngine;

public class ConatinerCounterVisual : MonoBehaviour
{
    private const String OPEN_CLOSE = "OpenClose";
    private Animator animator;
    [SerializeField] private ContainerCounter containerCounter;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObjects;
    }
    private void ContainerCounter_OnPlayerGrabbedObjects(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
