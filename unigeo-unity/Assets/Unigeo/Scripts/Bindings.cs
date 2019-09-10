using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unigeo
{
    static class Bindings
    {
        [DllImport("libunigeo")]
        internal static extern Vector3 create_vector3();

        [DllImport("libunigeo")]
        internal static extern Vector3 catmull_romm_spline_point(in CatMullRommSpline spline, float t);

        [DllImport("libunigeo")]
        internal static extern Vector3 catmull_romm_spline_tangent(in CatMullRommSpline spline, float t);
    }
}
