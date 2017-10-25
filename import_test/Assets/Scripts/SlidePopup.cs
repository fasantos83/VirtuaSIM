using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePopup : MonoBehaviour {

    public Sprite[] slides;
    public AudioClip[] audios;

    private int currentSlideIdx = 0;
    public GameObject currentSlide;
    public AudioSource currentAudio;

    public GameObject startSlideButton;
    public GameObject nextSlideButton;
    public GameObject previousSlideButton;

    public GameObject availImage;
    public GameObject inUseImage;

    public int slideWithOutsideButton = 6;

    private void Start() {
        ChangeToSlide(currentSlideIdx);
    }

    public void ChangeToNextSlide(bool fromOutsideButton = false) {
        if ((currentSlideIdx == slideWithOutsideButton && fromOutsideButton) || currentSlideIdx != slideWithOutsideButton) {
            currentSlideIdx++;
            if (currentSlideIdx == slides.Length) {
                CloseSlidePopup();
                currentSlideIdx = 0;
            } else {
                ChangeToSlide(currentSlideIdx);
            }
        }
    }

    public void ChangeToPreviousSlide() {
        currentSlideIdx--;
        Mathf.Clamp(currentSlideIdx, 0, slides.Length);
        ChangeToSlide(currentSlideIdx);
    }

    public void ChangeToSlide(int idx) {
        if (slides != null && audios != null) {
            if (slides[idx] != null) {
                currentSlide.GetComponent<SpriteRenderer>().sprite = slides[idx];
            }
            if (audios[idx] != null) {
                currentAudio.clip = audios[idx];
                currentAudio.Play();
            }
        }
    }

    public void CloseSlidePopup() {
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        if (currentAudio != null) {
            currentAudio.Stop();
        }
        if(availImage != null) {
            availImage.SetActive(false);
        }
        if (inUseImage != null) {
            inUseImage.SetActive(false);
        }
    }

    private void OnEnable() {
        if (currentAudio != null) {
            currentAudio.Play();
        }
        if (availImage != null) {
            availImage.SetActive(true);
        }
        if (inUseImage != null) {
            inUseImage.SetActive(true);
        }
    }

    private void Update() {
        if (!currentAudio.isPlaying) {
            currentAudio.clip = null;
        }

        if (currentAudio != null) {
            bool showButtons = !currentAudio.isPlaying;

            startSlideButton.SetActive(currentSlideIdx == 0 ? showButtons : false);
            nextSlideButton.SetActive(currentSlideIdx == 0 ? false : (showButtons && currentSlideIdx != slideWithOutsideButton));
            previousSlideButton.SetActive(currentSlideIdx == 0 ? false : showButtons);
        }
    }
}
