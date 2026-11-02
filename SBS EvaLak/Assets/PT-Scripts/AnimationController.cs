using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetHurt()
    {
        animator.SetBool("isHurt", true);
    }
}
