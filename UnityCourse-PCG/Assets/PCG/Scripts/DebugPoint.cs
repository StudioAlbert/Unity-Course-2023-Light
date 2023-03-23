using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    [ExecuteInEditMode]

    public class DebugPoint : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            RenamePoints(null);
        }

        private void OnDestroy()
        {
            RenamePoints(this);
        }

        // Update is called once per frame
        void Update()
        {
            TextMesh textMesh = GetComponentInChildren<TextMesh>();
            if(textMesh != null)
                textMesh.text = name;
        }

        void RenamePoints(DebugPoint mySelf)
        {
            DebugPoint[] debugPoints;
            debugPoints = FindObjectsOfType<DebugPoint>();

            int i = 1;
            foreach (DebugPoint debugPoint in debugPoints)
            {
                if (debugPoint != mySelf)
                {
                    debugPoint.name = "Point " + string.Format("{0:000}", i);
                    i++;
                }
            }
        }
    }
}
