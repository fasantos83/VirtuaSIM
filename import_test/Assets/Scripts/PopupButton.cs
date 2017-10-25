using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PopupButton : MonoBehaviour {

    private Vector3 startingPosition;
    public GameObject[] positions;
    public GameObject[] positionPopups;

    private bool gazedAt = false;
    private bool canMove = false;
    private bool popupShown = false;

    public float timeToShowPopup = 2f;
    public float timeToHidePopup = 5f;
    private float timer = 0f;
    private int currentPos = 0;
    public int offPosition = 1;

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
        CheckShowPopup();
    }

    private void CheckShowPopup() {
        if (gazedAt && !popupShown) {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToShowPopup);
            if (timer >= timeToShowPopup) {
                canMove = true;
                ShowPostitionPopups(true);
            }
        } else if (!gazedAt && popupShown) {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToHidePopup);
            if (timer >= timeToHidePopup) {
                ShowPostitionPopups(false);
            }
        } else if (!canMove && popupShown) {
            ShowPostitionPopups(false);
        }
    }

    public void SetGazedAt(bool toggle) {
        gazedAt = toggle;
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

    public void ShowPostitionPopups(bool toggle) {
        timer = 0;
        popupShown = toggle;
        if (positionPopups != null) {
            for (int i = 0; i < positionPopups.Length; i++) {
                if (positionPopups[i] != null) {
                    positionPopups[i].SetActive(toggle);
                }
            }
        }
    }

    public void MoveButtonToPosition(int idx) {
        if (canMove && positions != null) {
            for (int i = 0; i < positions.Length; i++) {
                GameObject button = positions[i];
                if (button != null) {
                    if (i == idx) {
                        button.SetActive(true);
                        AudioSource audio = button.GetComponent<AudioSource>();
                        if (audio != null) {
                            audio.Play();
                        }
                        canMove = false;
                        if (button.CompareTag("bt_disc")){
                            StartCoroutine(ChangeDiscToOff());
                        }
                    } else {
                        button.SetActive(false);
                    }
                }
            }
        }
    }

    public IEnumerator ChangeDiscToOff() {
        yield return new WaitForSeconds(1f);
        canMove = true;
        MoveButtonToPosition(offPosition);
    }
  
}
