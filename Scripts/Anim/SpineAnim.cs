using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class SpineAnim : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public SkeletonGraphic skeletonGraphic; // Added for UI animations
    public List<StateNameToAnimationReference> statesAndAnimations = new List<StateNameToAnimationReference>();
    public List<AnimationTransition> transitions = new List<AnimationTransition>(); // Alternately, an AnimationPair-Animation Dictionary (commented out) can be used for more efficient lookups.

    [System.Serializable]
    public class StateNameToAnimationReference 
    {
        public string stateName;
        public AnimationReferenceAsset animation;
    }

    [System.Serializable]
    public class AnimationTransition 
    {
        public AnimationReferenceAsset from;
        public AnimationReferenceAsset transition;
        public AnimationReferenceAsset to;
    }
    
    public Spine.Animation TargetAnimation { get; private set; }

    void Awake () 
    {
        // Initialize AnimationReferenceAssets
        foreach (var entry in statesAndAnimations) 
        {
            entry.animation.Initialize();
        }
        
        foreach (var entry in transitions) 
        {
            entry.from.Initialize();
            entry.to.Initialize();
            entry.transition.Initialize();
        }

        // Build Dictionary
        //foreach (var entry in transitions) {
        //    transitionDictionary.Add(new AnimationStateData.AnimationPair(entry.from.Animation, entry.to.Animation), entry.transition.Animation);
        //}
    }

    /// <summary>Sets the horizontal flip state of the skeleton based on a nonzero float. If negative, the skeleton is flipped. If positive, the skeleton is not flipped.</summary>
    public void SetFlip (float horizontal) 
    {
        if (horizontal != 0) {
            if (skeletonAnimation != null)
                skeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
            if (skeletonGraphic != null)
                skeletonGraphic.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
        }
    }

    /// <summary>Plays an animation based on the state name.</summary>
    public void PlayAnimationForState (string stateShortName, int layerIndex, bool doTransition = true) 
    {
        PlayAnimationForState(StringToHash(stateShortName), layerIndex, doTransition);
    }

    /// <summary>Plays an animation based on the hash of the state name.</summary>
    public void PlayAnimationForState (int shortNameHash, int layerIndex, bool doTransition) 
    {
        var foundAnimation = GetAnimationForState(shortNameHash);
        if (foundAnimation == null)
        {
            return;
        }

        PlayNewAnimation(foundAnimation, layerIndex, doTransition);
    }

    /// <summary>Gets a Spine Animation based on the state name.</summary>
    public Spine.Animation GetAnimationForState (string stateShortName) 
    {
        return GetAnimationForState(StringToHash(stateShortName));
    }

    /// <summary>Gets a Spine Animation based on the hash of the state name.</summary>
    public Spine.Animation GetAnimationForState (int shortNameHash) 
    {
        var foundState = statesAndAnimations.Find(entry => StringToHash(entry.stateName) == shortNameHash);
        return (foundState == null) ? null : foundState.animation.Animation;
    }

    /// <summary>Play an animation. If a transition animation is defined, the transition is played before the target animation being passed.</summary>
    public void PlayNewAnimation (Spine.Animation target, int layerIndex, bool doTransition) 
    {
        if (doTransition)
        {
            // Trace.Log("DOING TRANSITION");
            
            Spine.Animation transition = null;
            Spine.Animation current = null;

            current = GetCurrentAnimation(layerIndex);
            
            if (current != null)
                transition = TryGetTransition(current, target);

            if (transition != null) 
            {
                if (skeletonAnimation != null)
                {
                    skeletonAnimation.AnimationState.SetAnimation(layerIndex, transition, false);
                    skeletonAnimation.AnimationState.AddAnimation(layerIndex, target, true, 0f);
                }
                if (skeletonGraphic != null)
                {
                    skeletonGraphic.AnimationState.SetAnimation(layerIndex, transition, false);
                    skeletonGraphic.AnimationState.AddAnimation(layerIndex, target, true, 0f);
                }
            } 
            else 
            {
                if (skeletonAnimation != null)
                    skeletonAnimation.AnimationState.SetAnimation(layerIndex, target, true);
                if (skeletonGraphic != null)
                    skeletonGraphic.AnimationState.SetAnimation(layerIndex, target, true);
            }    
        }
        else
        {
            if (skeletonAnimation != null)
                skeletonAnimation.AnimationState.SetAnimation(layerIndex, target, true);
            if (skeletonGraphic != null)
                skeletonGraphic.AnimationState.SetAnimation(layerIndex, target, true);
        }
        
        this.TargetAnimation = target;
    }

    /// <summary>Play a non-looping animation once then continue playing the state animation.</summary>
    public void PlayOneShot (string id, int layerIndex)
    {
        Spine.Animation oneShot = null;
        
        foreach (var stateNameToAnimationReference in statesAndAnimations)
        {
            if (stateNameToAnimationReference.stateName == id)
            {
                oneShot = stateNameToAnimationReference.animation.Animation;
            }
        }
        var state = skeletonAnimation != null ? skeletonAnimation.AnimationState : skeletonGraphic.AnimationState;
        state.SetAnimation(0, oneShot, false);

        var transition = TryGetTransition(oneShot, TargetAnimation);
        if (transition != null)
        {
            state.AddAnimation(0, transition, false, 0f);
        }

        state.AddAnimation(0, this.TargetAnimation, true, 0f);
    }

    /// <summary>Play a non-looping animation once then continue playing the state animation with a different TimeScale.</summary>

    public void PlayOneShotNoTransition(string id, int layerIndex, float speed = 1)
    {
        Spine.Animation oneShot = null;

        foreach (var stateNameToAnimationReference in statesAndAnimations)
        {
            if (stateNameToAnimationReference.stateName == id)
            {
                oneShot = stateNameToAnimationReference.animation.Animation;
            }
        }
        var state = skeletonAnimation != null ? skeletonAnimation.AnimationState : skeletonGraphic.AnimationState;
        state.SetAnimation(0, oneShot, false);
        state.GetCurrent(0).TimeScale = speed;
        state.AddAnimation(0, this.TargetAnimation, true, 0f);
    }

    Spine.Animation TryGetTransition (Spine.Animation from, Spine.Animation to) 
    {
        foreach (var transition in transitions) 
        {
            if (transition.from.Animation == from && transition.to.Animation == to) 
            {
                return transition.transition.Animation;
            }
        }
        
        return null;

        //Spine.Animation foundTransition = null;
        //transitionDictionary.TryGetValue(new AnimationStateData.AnimationPair(from, to), out foundTransition);
        //return foundTransition;
    }

    public Spine.Animation GetCurrentAnimation (int layerIndex) 
    {
        var currentTrackEntry = skeletonAnimation != null ? skeletonAnimation.AnimationState.GetCurrent(layerIndex) : skeletonGraphic.AnimationState.GetCurrent(layerIndex);
        return (currentTrackEntry != null) ? currentTrackEntry.Animation : null;
    }

    int StringToHash (string s) 
    {
        return Animator.StringToHash(s);
    }
    
    // * =====================================================================================================================================
    // * 
    
    public void PlayAnimationForStateAtProgress(string stateShortName, int layerIndex, bool doTransition, float startPercentage)
    {
        PlayAnimationForStateAtProgress(StringToHash(stateShortName), layerIndex, doTransition, startPercentage);
    }
    
    public void PlayAnimationForStateAtProgress(int shortNameHash, int layerIndex, bool doTransition, float startPercentage)
    {
        var foundAnimation = GetAnimationForState(shortNameHash);
        if (foundAnimation == null)
            return;
    
        PlayNewAnimationAtProgress(foundAnimation, layerIndex, doTransition, startPercentage);
    }
    
    public void PlayNewAnimationAtProgress(Spine.Animation target, int layerIndex, bool doTransition, float startPercentage)
    {
        // Asegúrate de que el porcentaje esté dentro de los límites
        startPercentage = Mathf.Clamp(startPercentage, 0f, 100f);
    
        var spineState = (skeletonAnimation != null) ? skeletonAnimation.AnimationState : skeletonGraphic.AnimationState;
        TrackEntry finalTrackEntry = null;
    
        if (doTransition)
        {
            Spine.Animation transition = null;
            Spine.Animation current = GetCurrentAnimation(layerIndex);
            if (current != null)
                transition = TryGetTransition(current, target);
    
            if (transition != null)
            {
                spineState.SetAnimation(layerIndex, transition, false);
                finalTrackEntry = spineState.AddAnimation(layerIndex, target, true, 0f);
            }
            else
            {
                finalTrackEntry = spineState.SetAnimation(layerIndex, target, true);
            }
        }
        else
        {
            finalTrackEntry = spineState.SetAnimation(layerIndex, target, true);
        }
    
        this.TargetAnimation = target;
    
        // Calcula el tiempo inicial según el porcentaje
        float startTime = target.Duration * (startPercentage / 100f);
        if (finalTrackEntry != null)
        {
            // Asigna el tiempo inicial de la animación
            finalTrackEntry.TrackTime = startTime;
        }
    }
}
