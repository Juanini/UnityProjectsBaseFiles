using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TweenConfigSO", menuName = GameConst.SO_PATH + "TweenConfigSO", order = 0)]
public class TweenConfigSO : ScriptableObject
{
    public float time;
    public float to;
    public float from;
    public Ease easeType;
}