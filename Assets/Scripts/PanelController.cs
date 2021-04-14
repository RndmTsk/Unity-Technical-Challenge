using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    // An animator with an "isShowing" animation control
    private Animator panelAnimator;

    void Start() {
        panelAnimator = GetComponent<Animator>();
    }

    public void Show() {
        panelAnimator.SetBool("isHiding", false);
        panelAnimator.SetBool("isShowing", true);
    }

    public void Hide() {
        panelAnimator.SetBool("isShowing", false);
        panelAnimator.SetBool("isHiding", true);
    }
}
