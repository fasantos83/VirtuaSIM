using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUButton : MonoBehaviour {

    public GameObject slidePopup;
    public GameObject image;

    private void OnEnable() {
        image.SetActive(slidePopup.activeSelf);
    }
}
