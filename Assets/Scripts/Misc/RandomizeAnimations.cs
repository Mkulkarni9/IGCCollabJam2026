using UnityEngine;

public class RandomizeAnimations : MonoBehaviour
{
    Animator animator;


    [SerializeField]AnimationClip clip;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        RandomizeAnimation();
    }


    void RandomizeAnimation()
    {
        float randomNormalizedTime = Random.value;

        animator.Play(clip.name, 0, randomNormalizedTime);

        animator.Update(0f);
    }
}
