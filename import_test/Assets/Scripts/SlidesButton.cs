using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class SlidesButton : MonoBehaviour {

    private Vector3 startingPosition;
    public GameObject slidePopup;

    private bool gazedAt = false;
    private bool slidePopupOpen = false;
    private bool canChange = true;

    private float timer = 0f;
    public float timeToShow = 2f;

    public AudioSource onOffSound;

    void Start() {
        startingPosition = transform.localPosition;
    }

    public void Reset() {
        transform.localPosition = startingPosition;
    }

    public void Recenter() {
#if !UNITY_EDITOR
        GvrCardboardHelpers.Recenter();
#else
        GvrEditorEmulator emulator = FindObjectOfType<GvrEditorEmulator>();
        if (emulator == null) {
            return;
        }
        emulator.Recenter();
#endif  // !UNITY_EDITOR
    }

    private void Update() {
        CheckMoveButton();
    }

    private void CheckMoveButton() {
        if (gazedAt && canChange) {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToShow);
            if (timer >= timeToShow) {
                ShowSlidePopup();
            }
        }
    }

    public void SetGazedAt(bool toggle) {
        gazedAt = toggle;
        if (!gazedAt) canChange = true;
        ChangeButtonColor();
    }

    private void ChangeButtonColor() {
        Color32 col = gazedAt ? Color.green : Color.gray;
        if (gazedAt) {
            col.a = 100;
        } else {
            col.a = 0;
        }
        foreach (Transform child in transform) {
            if (!child.gameObject.CompareTag("popup")) {
                Renderer r = child.gameObject.GetComponent<Renderer>();
                if (r != null) {
                    r.material.color = col;
                }
            }
        }
    }

    public void ShowSlidePopup() {
        PlayOnOffSound();
        canChange = false;
        slidePopupOpen = !slidePopupOpen;
        if (slidePopup != null) {
            timer = 0;
            slidePopup.SetActive(slidePopupOpen);
        }
    }

    public void PlayOnOffSound() {
        if (onOffSound != null) {
            onOffSound.Play();
        }
    }



}
