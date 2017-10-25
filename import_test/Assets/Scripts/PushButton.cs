using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class PushButton : MonoBehaviour {

    private Vector3 startingPosition;
    public GameObject[] positions;

    private bool gazedAt = false;
    private bool canMove = true;

    private float timer = 0f;
    public float timeToMove = 2f;

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
        CheckMoveButton();
    }

    private void CheckMoveButton() {
        if (gazedAt && canMove) {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, timeToMove);
            if (timer >= timeToMove) {
                MoveToNextPosition();
            }
        }
    }

    public void SetGazedAt(bool toggle) {
        gazedAt = toggle;
        if(!canMove && !gazedAt) {
            canMove = true;
        }
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

    public void MoveButtonToPosition(int idx) {
        if (positions != null) {
            for (int i = 0; i < positions.Length; i++) {
                GameObject button = positions[i];
                AudioSource audio = button.GetComponent<AudioSource>();
                if (button != null && audio != null) {
                    bool toggle = (i == idx);
                    button.SetActive(toggle);
                    audio.Play();
                }
            }
            canMove = false;
        }
    }

    public void MoveToNextPosition() {
        timer = 0;
        currentPos++;
        if (currentPos == positions.Length) {
            currentPos = 0;
        }
        MoveButtonToPosition(currentPos);
    }


}
