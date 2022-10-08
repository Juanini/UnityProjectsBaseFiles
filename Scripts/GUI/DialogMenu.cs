using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        public GameObject container;
        public TextMeshProUGUI dialogText;
        public Button nextButton;
        public Button nextButtonFullScreen;

        private Sequence dialogTextSequence;

        private UnityAction nextClickCallback;

        [BoxGroup("Positions")] public GameObject posUp; 
        [BoxGroup("Positions")] public GameObject posCenter; 
        [BoxGroup("Positions")] public GameObject posDown;

        private Vector3 punchV = new Vector3(0.35f, 0.35f, 0.35f);

        public void ShowDialog(string _text, int _position = GameConst.POS_UP)
        {
            container.gameObject.SetActive(true);
            container.transform.DOPunchScale(punchV, 0.23f);
            SetText(_text);
        }
        
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
        public async void SetText(string _text)
        {
            if (dialogTextSequence.IsActive()) { dialogTextSequence.Kill(); }

            nextButtonFullScreen.interactable = false;
            AudioManager.Ins.PlayTextSound(true);
            
            dialogText.text = _text;
            dialogText.alpha = 0;
            
            DOTweenTMPAnimator animator = new DOTweenTMPAnimator(dialogText);

            for (int i = 0; i < animator.textInfo.characterCount; ++i) 
            {
                if (!animator.textInfo.characterInfo[i].isVisible) continue;
                await UniTask.Delay(35);
                dialogTextSequence.Join(animator.DOFadeChar(i, 1, 0.05f));
                dialogTextSequence.Join(animator.DOPunchCharScale(i, 0.33f, 0.23f).SetEase(Ease.InOutQuint));
            }
            
            nextButtonFullScreen.interactable = true;
            AudioManager.Ins.PlayTextSound(false);
        }

        public void OnNextClick()
        {
            nextClickCallback.Invoke();
        }

        public void SetNextClickCallback(UnityAction _callback)
        {
            nextClickCallback = _callback;
        }

        public override void OnViewCreated()
        {
            base.OnViewCreated();
            container.gameObject.SetActive(false);
            dialogTextSequence = DOTween.Sequence();

            nextButton.onClick.AddListener(OnNextClick);
            nextButtonFullScreen.onClick.AddListener(OnNextClick);
        }
    }
    
    
}
