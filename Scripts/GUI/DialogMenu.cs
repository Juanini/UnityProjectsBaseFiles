using DG.Tweening;
using Sirenix.OdinInspector;
using HannieEcho.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HannieEcho
{
    public class DialogMenu : UIView
    {
        [BoxGroup("Elements")] public GameObject container;
        [BoxGroup("Elements")] public TextMeshProUGUI dialogText;
        [BoxGroup("Elements")] public Button nextButton;
        [BoxGroup("Elements")] public Button nextButtonFullScreen;
        [BoxGroup("Elements")] public Image nextImage;

        private Sequence dialogTextSequence;

        private UnityAction nextClickCallback;

        [BoxGroup("Positions")] public GameObject posUp;
        [BoxGroup("Positions")] public GameObject posCenter; 
        [BoxGroup("Positions")] public GameObject posDown;

        private readonly Vector3 punchV = new Vector3(0.35f, 0.35f, 0.35f);

        // public async UniTask ShowDialog(string _text, bool _enableNext = true, int _position = GameConst.POS_UP)
        // {
        //     nextImage.gameObject.SetActive(false);
        //     nextButton.gameObject.SetActive(false);
        //     nextButtonFullScreen.gameObject.SetActive(false);
        //     
        //     switch (_position)
        //     {
        //         case GameConst.POS_UP:
        //             container.transform.position = posUp.transform.position;
        //             break;
        //     }
        //     
        //     container.gameObject.SetActive(true);
        //     container.transform.DOPunchScale(punchV, 0.23f);
        //     await SetText(_text);
        //     
        //     await UniTask.Delay(500);
        //
        //     nextImage.gameObject.SetActive(_enableNext);
        //     nextButton.gameObject.SetActive(_enableNext);
        //     nextButtonFullScreen.gameObject.SetActive(_enableNext);
        // }
        //
        // public void HideDialog()
        // {
        //     container.gameObject.SetActive(false);
        // }
        //
        // [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        // public async UniTask SetText(string _text)
        // {
        //     if (dialogTextSequence.IsActive()) { dialogTextSequence.Kill(); }
        //
        //     StringBuilder dialogTextString = new StringBuilder(_text);
        //
        //     nextButtonFullScreen.interactable = false;
        //     AudioManager.Ins.PlayTextSound(true);
        //     
        //     dialogText.text = _text;
        //     dialogText.alpha = 0;
        //     
        //     DOTweenTMPAnimator animator = new DOTweenTMPAnimator(dialogText);
        //
        //     for (int i = 0; i < animator.textInfo.characterCount; ++i) 
        //     {
        //         if (!animator.textInfo.characterInfo[i].isVisible) continue;
        //
        //         // TODO: Make a pause
        //         if (dialogText.textInfo.characterInfo[0].character == '_')
        //         {
        //             await UniTask.Delay(65);    
        //         }
        //         else
        //         {
        //             await UniTask.Delay(35);
        //         }
        //         
        //         dialogTextSequence.Join(animator.DOFadeChar(i, 1, 0.05f));
        //         dialogTextSequence.Join(animator.DOPunchCharScale(i, 0.33f, 0.23f).SetEase(Ease.InOutQuint));
        //     }
        //     
        //     nextButtonFullScreen.interactable = true;
        //     AudioManager.Ins.PlayTextSound(false);
        // }
        //
        // public void OnNextClick()
        // {
        //     AudioManager.Ins.UIClickSound();
        //     nextClickCallback.Invoke();
        // }

        // public void SetNextClickCallback(UnityAction _callback)
        // {
        //     nextClickCallback = _callback;
        // }
        //
        // public override void OnViewCreated()
        // {
        //     base.OnViewCreated();
        //     container.gameObject.SetActive(false);
        //     dialogTextSequence = DOTween.Sequence();
        //
        //     nextButton.onClick.AddListener(OnNextClick);
        //     nextButtonFullScreen.onClick.AddListener(OnNextClick);
        // }
    }
    
    
}
