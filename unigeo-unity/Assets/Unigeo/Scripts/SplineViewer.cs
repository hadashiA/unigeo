using UnityEngine;

namespace Unigeo
{
    public class SplineViewer : MonoBehaviour
    {
        [SerializeField]
        public Spline Spline = new CatMullRommSpline(false);

        [SerializeField]
        int steps;
    }
}