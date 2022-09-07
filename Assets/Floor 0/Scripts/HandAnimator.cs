using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimator : MonoBehaviour
{
    [SerializeField]
    InputActionReference joystickTouch;

    [SerializeField]
    private ActionBasedController controller;

    [SerializeField]
    private float speed;

    private Animator animator;
    private float gripTarget;
    private float gripCurrent;
    private float triggerTarget;
    private float triggerCurrent;
    private float thumbCurrent;
    private bool thumbState = false;

    private string animatorGrip = "Grip";
    private string animatorTrigger = "Trigger";
    private string animatorThumb = "Thumb";
    private void Awake()
    {
        joystickTouch.action.started += Toggle;
        joystickTouch.action.canceled += Toggle;

    }
    private void OnDestroy()
    {
        joystickTouch.action.started -= Toggle;
        joystickTouch.action.canceled -= Toggle;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        gripTarget =  controller.selectAction.action.ReadValue<float>();
        triggerTarget = controller.activateAction.action.ReadValue<float>();

        AnimateHand();
    }

    private void AnimateHand()
    {
        if (gripTarget != gripCurrent)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGrip, gripCurrent);
        }
        if (triggerTarget != triggerCurrent)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorTrigger, triggerCurrent);
        }
        if (thumbState)
        {
            thumbCurrent = Mathf.MoveTowards(thumbCurrent, 1f, Time.deltaTime * speed);
            animator.SetFloat(animatorThumb, thumbCurrent);
        }
        else
        {
            thumbCurrent = Mathf.MoveTowards(thumbCurrent, 0f, Time.deltaTime * speed);
            animator.SetFloat(animatorThumb, thumbCurrent);
        }
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        thumbState = !thumbState;
    }
}
