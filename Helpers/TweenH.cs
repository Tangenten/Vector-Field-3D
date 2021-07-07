using System;

namespace Helpers {
	public static class TweenH {
		// fraction is between 0 and 1
		public static double Linear(double fromMin, double fromMax, double fraction){
			return (fromMin * (1.0 - fraction) + fromMax * fraction);
		}

		// fraction is between 0 and 1
		public static double Cosine(double fromMin, double fromMax, double fraction){
			fraction = (1.0 - Math.Cos(fraction * Math.PI)) / 2.0;
			return (fromMin * (1.0 - fraction) + fromMax * fraction);
		}

		// scales a value from one scale to another scale linearly
		public static double Linear(double fromValue, double fromMin, double fromMax, double toMin, double toMax) {
			return Linear(toMin, toMax, (fromValue - fromMin) / (fromMax - fromMin));
		}

		// scales a value from one scale to another scale using cosine function (smoother)
		public static double Cosine(double fromValue, double fromMin, double fromMax, double toMin, double toMax) {
			return Cosine(toMin, toMax, (fromValue - fromMin) / (fromMax - fromMin));
		}

		public static void SmoothToTarget(ref float current, float target, float scalar) {
			current += (target - current) / scalar;
		}

		public static float SmoothToTarget(float current, float target, float scalar) {
			current += (target - current) / scalar;
			return current;
		}

		public static void ExponentialSmoothing(ref float input, float scalar) {
			input *= scalar;
		}

		public static float ExponentialSmoothing(float input, float scalar) {
			input *= scalar;
			return input;
		}

		public static void LinearSmoothing(ref float input, float scalar) {
			input += scalar;
		}

		public static float LinearSmoothing(float input, float scalar) {
			input += scalar;
			return input;
		}

	}
}
