using UnityEngine;

// This is the base class for most the other menu scripts

public class UIMenuBase : MonoBehaviour {

    public Animator animator;

    public virtual void Start() {
        animator.Play("Closed");
    }

    public virtual void OpenMenu() {
        animator.Play("Fade & Slide In");
    }
}