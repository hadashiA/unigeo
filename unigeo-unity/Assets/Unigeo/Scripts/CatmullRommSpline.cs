﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unigeo
{
    [System.Serializable]
    public abstract class Spline
    {
        [SerializeField]
        public List<Vector3> ControlPoints = new List<Vector3>
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f),
        };

        [SerializeField]
        public bool Closed;

        public int CurveCount => (ControlPoints.Count - 1) / 3;

        public Spline(bool closed)
        {
            Closed = closed;
        }

        public abstract Vector3 GetPoint(float t);
        public abstract Vector3 GetTangent(float t);

        public void AddControlPoint(Vector3 point)
        {
            ControlPoints.Add(Vector3.zero);
            SetControlPoint(ControlPoints.Count - 1, point);
        }

        public void AddCurve()
        {
            var lastIndex = ControlPoints.Count - 1;
            var lastPoint = ControlPoints[lastIndex];
            lastPoint.x += 1f;
            ControlPoints.Add(new Vector3(lastPoint.x + 1, lastPoint.y, lastPoint.z));
            ControlPoints.Add(new Vector3(lastPoint.x + 2, lastPoint.y, lastPoint.z));
            ControlPoints.Add(new Vector3(lastPoint.x + 3, lastPoint.y, lastPoint.z));
//
//            var lastMode = ControlPointModes.Last();
//            ControlPointModes.Add(lastMode);
//            EnforceMode(lastIndex);
        }

        public void Clear()
        {
            ControlPoints.Clear();
        }

        public Vector3 GetControlPoint(int i)
        {
            return ControlPoints[i];
        }

        public void SetControlPoint(int i, Vector3 point)
        {
            ControlPoints[i] = point;
        }
    }

    struct HermitePolynominal
        {
        public Vector3 P0;
        public Vector3 P1;
        public Vector3 V0;
        public Vector3 V1;
        public float T;

        public Vector3 GetPoint()
        {
            var c0 = 2f * P0 + -2f * P1 + V0 + V1;
            var c1 = -3f * P0 + 3f * P1 + -2f * V0 - V1;
            var c2 = V0;
            var c3 = P0;

            var t2 = T * T;
            var t3 = t2 * T;
            return c0 * t3 + c1 * t2 + c2 * T + c3;
        }

        public Vector3 GetTangent()
        {
            var c0 = 6f * P0 - 6f * P1 + 3f * V0 + 3f * V1;
            var c1 = -6f * P0 + 6f * P1 - 4f * V0 - 2f * V1;
            var c2 = V0;

            var t2 = T * T;
            return c0 * t2 + c1 * T + c2;
        }
    }

    [Serializable]
    public class CatMullRommSpline : Spline
    {
        public CatMullRommSpline(bool closed) : base(closed)
        {
        }

        public override Vector3 GetPoint(float t)
        {
            return GetPolynominal(t).GetPoint();
        }

        public override Vector3 GetTangent(float t)
        {
            return GetPolynominal(t).GetTangent();
        }

        HermitePolynominal GetPolynominal(float t)
        {
            var l = ControlPoints.Count;
            var progress = (l - (Closed ? 0 : 1)) * t;
            var i = Mathf.FloorToInt(progress);
            var weight = progress - i;

            if (!Closed && Mathf.Approximately(weight, 0f) && i >= l - 1)
            {
                i = l - 2;
                weight = 1;
            }

            Vector3 p0, p1;
            if (Closed && i >= l - 1)
            {
                // last to first point
                p0 = ControlPoints[l - 1];
                p1 = ControlPoints[0];
            }
            else
            {
                p0 = ControlPoints[i];
                p1 = ControlPoints[i + 1];
            }

            Vector3 v0;
            if (i > 0)
            {
                // prev to next point
                v0 = 0.5f * (p1 - ControlPoints[i - 1]);
            }
            else if (Closed)
            {
                // last to next point
                v0 = 0.5f * (p1 - ControlPoints[l - 1]);
            }
            else
            {
                v0 = p1 - p0;
            }

            Vector3 v1;
            if (i < l - 2)
            {
                v1 = 0.5f * (ControlPoints[i + 2] - p0);
            }
            else if (Closed)
            {
                if (i >= l - 1)
                {
                    // last to second point
                    v1 = 0.5f * (ControlPoints[1] - p0);
                }
                else
                {
                    // second last to first point
                    v1 = 0.5f * (ControlPoints[0] - p0);
                }
            }
            else
            {
                v1 = p1 - p0;
            }

            return new HermitePolynominal
            {
                P0 = p0,
                P1 = p1,
                V0 = v0,
                V1 = v1,
                T = weight
            };
        }
    }
}
