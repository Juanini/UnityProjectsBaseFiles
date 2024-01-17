using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(menuName = "Anim", fileName = "SpineAnimData", order = 0)]
[InlineEditor]
public class SpineAnimData : ScriptableObject
{
    public List<StateNameToAnimationReference> statesAndAnimations = new List<StateNameToAnimationReference>();
    
    // Alternately, an AnimationPair-Animation Dictionary (commented out) can be used for more efficient lookups.
    public List<AnimationTransition> transitions = new List<AnimationTransition>();
    
    // [ShowInInspector]
    // public Dictionary<Spine.AnimationStateData.AnimationPair, Spine.Animation> transitionDictionary 
    //     = new Dictionary<AnimationStateData.AnimationPair, Spine.Animation>(Spine.AnimationStateData.AnimationPairComparer.Instance);
    
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
}
