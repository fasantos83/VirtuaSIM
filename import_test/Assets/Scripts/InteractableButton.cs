using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class InteractableButton : MonoBehaviour {

    private Vector3 startingPosition;
    public GameObject[] positions;
    public GameObject[] positionPopups;

    private bool gazedAt = false;
    private bool canMove = false;

    public float timeToShowPopup = 2f;
    private float timer = 0f;
    private int currentPos = 0;

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
        if (gazedAt) {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToShowPopup);
            if (timer >= timeToShowPopup) {
                ShowPostitionPopups(true);
            }
        } else {
            timer -= Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToShowPopup);
            if (timer == 0) {
                ShowPostitionPopups(false);
            }
        }
    }

    public void SetGazedAt(bool toggle) {
        gazedAt = toggle;

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

        if (!gazedAt) {
            timer = timeToShowPopup;
        }
    }

    public void ShowPostitionPopups(bool toggle) {
        canMove = toggle;
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
                    bool toggle = (i == idx);
                    button.SetActive(toggle);
                    AudioSource audio = button.GetComponent<AudioSource>();
                    if(audio != null) {
                        audio.Play();
                    }
                }
            }
        }
        ShowPostitionPopups(false);
        SetGazedAt(false);
    }

    public void MoveButtonToNextPosition() {
        currentPos++;
        if (currentPos == positions.Length) {
            currentPos = 0;
        }
        MoveButtonToPosition(currentPos);
    }
}
