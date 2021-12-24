using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {

    [SerializeField]
    RuntimeAnimatorController shineAnimator;

    [SerializeField]
    AnimationClip animationClip;

    public Animator[] tagAnimators;
    public MeshRenderer[] tagRenderer;

    private int stateIndex=-1;

    public void InvokeTagShine(float time)
    {
        Invoke("TagShine", time);
        stateIndex = StateController.Instance.stateIndex;


        float animationLength = animationClip.length;


        Invoke("SetTagMateiralsBack", animationLength + time);
    }

    private void CloseAniamtion()
    {

        SetTagMateiralsBack();
        Debug.Log("close animation is called");
    }


    public void TagShine()
    {
        Debug.Log("tagsshine invoke");
        if (stateIndex == StateController.Instance.stateIndex)
            foreach (Animator anime in tagAnimators)
            {
                anime.runtimeAnimatorController = shineAnimator;
            }
        else
        {
            SetTagMateiralsBack();
        }
    }

    public void SetTagMateiralsBack()
    {
        CancelInvoke();
        foreach (MeshRenderer renderer in tagRenderer)
        {
            renderer.material.color = Color.white;
            if (renderer.material.HasProperty("_EMISSION"))
            {
                renderer.material.DisableKeyword("_EMISSION");
            }
            else
            {
                Debug.Log("Emission property not found");
            }
        }
        foreach (Animator anime in tagAnimators)
        {
            anime.runtimeAnimatorController = null;

        }

    }



}
