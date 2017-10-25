using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour {

    public GameObject popup;

    public void ClosePopup() {
        StartCoroutine(ClosePopupRoutine());
    }
    
    public IEnumerator ClosePopupRoutine() {
        yield return new WaitForSeconds(1f);
        popup.SetActive(false);
    }
}
