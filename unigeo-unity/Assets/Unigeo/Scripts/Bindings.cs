using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unigeo
{
    public class CatmullRommSplineRust : IDisposable
    {
        public bool Closed => Bindings.catmull_romm_spline_closed(ptr);
        public int ControlPointCount => Bindings.catmull_romm_spline_control_point_count(ptr);

        readonly IntPtr ptr;

        public CatmullRommSplineRust(bool closed)
        {
            ptr = Bindings.catmull_romm_spline_new(closed);
        }

        public void Dispose()
        {
            Bindings.catmull_romm_spline_drop(ptr);
        }

        public void AddControlPoint(Vector3 p)
        {
            Bindings.catmull_romm_spline_add_control_point(ptr, p);
        }

        public Vector3 GetControlPoint(int i)
        {
            return Bindings.catmull_romm_spline_get_control_point(ptr, (ulong)i);
        }

        public void SetControlPoint(int i, Vector3 p)
        {
            Bindings.catmull_romm_spline_set_control_point(ptr, (ulong)i, p);
        }

        public Vector3 GetPoint(float t)
        {
            return Bindings.catmull_romm_spline_calculate_point(ptr, t);
        }

//        public void AddCurve()
//        {
//            var lastIndex = ControlPoints.Count - 1;
//            var lastPoint = ControlPoints[lastIndex];
//            lastPoint.x += 1f;
//            ControlPoints.Add(new Vector3(lastPoint.x + 1, lastPoint.y, lastPoint.z));
//            ControlPoints.Add(new Vector3(lastPoint.x + 2, lastPoint.y, lastPoint.z));
//            ControlPoints.Add(new Vector3(lastPoint.x + 3, lastPoint.y, lastPoint.z));
//        }
    }

    static class Bindings
    {
        [DllImport("libunigeo")]
        internal static extern IntPtr catmull_romm_spline_new(bool closed);

        [DllImport("libunigeo")]
        internal static extern void catmull_romm_spline_drop(IntPtr ptr);

        [DllImport("libunigeo")]
        internal static extern bool catmull_romm_spline_closed(IntPtr ptr);

        [DllImport("libunigeo")]
        internal static extern int catmull_romm_spline_control_point_count(IntPtr ptr);

        [DllImport("libunigeo")]
        internal static extern void catmull_romm_spline_add_control_point(IntPtr ptr, Vector3 p);

        [DllImport("libunigeo")]
        internal static extern Vector3 catmull_romm_spline_get_control_point(IntPtr ptr, ulong i);

        [DllImport("libunigeo")]
        internal static extern void catmull_romm_spline_set_control_point(IntPtr ptr, ulong i, Vector3 p);

        [DllImport("libunigeo")]
        internal static extern Vector3 catmull_romm_spline_calculate_point(IntPtr ptr, float t);
    }
}
