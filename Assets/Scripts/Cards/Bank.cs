using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Bank : CardHub {
    public static Bank instance;

    private void Awake() {
        instance = this;
    }
}
