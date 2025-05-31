using DG.Tweening;
using ProcRanking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowtoPanel : MonoBehaviour
{
    bool isPanel = false;
    [SerializeField] GameObject howtoButtonObj;
    Button howtoButton;
    Image howtoButtonImage;

    [SerializeField] Sprite howtoButtonSprite;
    [SerializeField] Sprite backButtonSprite;
    [SerializeField] Sprite howtoButtonSprite_touch;
    [SerializeField] Sprite backButtonSprite_touch;

    AudioSource audioSource;
    public AudioClip clickSound;

    [SerializeField] GameObject endlessReceipt;
    RankingReceipt rankingReceipt;

    [SerializeField] List<Button> InteractableButtons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        howtoButtonImage = howtoButtonObj.GetComponent<Image>();
        howtoButton = howtoButtonObj.GetComponent<Button>();

        audioSource = GetComponent<AudioSource>();
        rankingReceipt = endlessReceipt.GetComponent<RankingReceipt>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HowtoButtonClicked()
    {
        SoundManager.soundManager.SEPlay(SEType.ButtonClick);

        if (!isPanel)
        {
            isPanel = true;
            foreach (var button in InteractableButtons)
            {
                button.interactable = false;
            }

            this.transform.DOLocalMove(new Vector3(0f, 18f, 0f), 0.4f).SetEase(Ease.InOutSine);

            if (rankingReceipt.isRanking)
            {
                rankingReceipt.E_ReceiptDown();
            }

            howtoButtonImage.sprite = backButtonSprite;

            SpriteState spriteState = howtoButton.spriteState;
            spriteState.highlightedSprite = backButtonSprite_touch;
            howtoButton.spriteState = spriteState;
        }
        else
        {
            isPanel = false;
            foreach (var button in InteractableButtons)
            {
                button.interactable = true;
            }
            this.transform.DOLocalMove(new Vector3(0f, 420f, 0f), 0.4f).SetEase(Ease.InOutSine);


            howtoButtonImage.sprite = howtoButtonSprite;

            SpriteState spriteState = howtoButton.spriteState;
            spriteState.highlightedSprite = howtoButtonSprite_touch;
            howtoButton.spriteState = spriteState;
        }
    }
}
