using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideSlideButton : MonoBehaviour {

    public SlidePopup slidePopup;

    public void OnEnable() {
        slidePopup.ChangeToNextSlide(true);
    }
}
