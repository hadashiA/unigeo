using System;
using System.Collections;
using UnityEngine;

namespace Unigeo
{
    public class SplineViewer : MonoBehaviour
    {
        [SerializeField]
        // public CatMullRommSpline Spline = new CatMullRommSpline(false);
        public CatmullRommSplineRust Spline = new CatmullRommSplineRust(false);

        [SerializeField]
        int steps = 100;

        void Start()
        {
            StartCoroutine(MoveAsync());
        }

        IEnumerator MoveAsync()
        {
            var p1 = Spline.GetControlPoint(1);
            var p2 = Spline.GetControlPoint(2);
            var t = 0f;

            while (true)
            {
                t += Time.deltaTime;
                var sin = Mathf.Sin(t);
                var cos = Mathf.Cos(t);
                yield return null;
                Spline.SetControlPoint(1, p1 + new Vector3(0f, 0f, sin));
                Spline.SetControlPoint(2, p2 + new Vector3(0f, 0f, cos));
            }
        }
    }
}