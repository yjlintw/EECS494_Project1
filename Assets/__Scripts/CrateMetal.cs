﻿using UnityEngine;
using System.Collections;

public class CrateMetal : Crate {


    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == Tags.CRASH) {
            bool landed = Crash.S.collider.bounds.min.y >= boxCol.bounds.max.y - .1f;
            if(landed) {
                Crash.S.LandOnCrate();
            }
        }
    }
    
    void OnCollisionStay(Collision col) {
       
    }
    
    protected override void BreakBox() {
       
    }
}