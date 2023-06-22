
using UnityEngine;
using FishNet.Object;
using FishNet.Component.Animating;

public class AnimatorCharacter : NetworkBehaviour
{
    public NetworkAnimator networkAnimator;
    public Animator animator;
    public Character character;
    public CastCharacter caster;
    public InputCharacter input;

    private string currentState;

    [Header("Animation Names")]
    public string idle;
    public string walk;
    public string skill;
    public string dash;
    public string ultimate;

    private bool isCasting;

    private void Awake()
    {
        character = GetComponent<Character>();
        animator = character.animator;
        networkAnimator = GetComponent<NetworkAnimator>();
        input = character.input;
        caster = character.caster;
    }

    private void ChangeAnimation(string newState)
    {
        if (currentState == newState) return;
        networkAnimator.Play(newState);
        currentState = newState;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if (!isCasting)
        {
            if (input.velocity != Vector2.zero)
            {
                ChangeAnimation(walk);
            }
            else
            {
                ChangeAnimation(idle);
            }
        }
    }

    public void PlaySkill()
    {
        PlayCastAnimation(skill);
    }

    public void PlayDash()
    {
        PlayCastAnimation(dash);
    }

    public void PlayUltimate()
    {
        PlayCastAnimation(ultimate);
    }

    private void PlayCastAnimation(string name)
    {
        if (!IsOwner) return;
        isCasting = true;
        ChangeAnimation(name);
        float delay = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(StoppedCasting), delay);
    }
    private void StoppedCasting()
    {
        isCasting = false;
    }
}
