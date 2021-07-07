using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

namespace Helpers {
	public static class Vector {
		public static float DistanceBetweenPoints(in Vector3 v1, in Vector3 v2) {
			return (float) Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2) + Math.Pow(v2.Z - v1.Z, 2));
		}

		public static float DotProduct(Vector3 v1, Vector3 v2) {
			float sum = 0f;

			v1 = NormalizeVector(v1);
			v2 = NormalizeVector(v2);

			sum += v1.X * v2.X;
			sum += v1.Y * v2.Y;
			sum += v1.Z * v2.Z;

			return sum;
		}

		public static Vector3 NormalizeVector(Vector3 v1) {
			float length = (float) Math.Sqrt((v1.X * v1.X) + (v1.Y * v1.Y) + (v1.Z * v1.Z));

			return new Vector3(v1.X / length, v1.Y / length, v1.Z / length);
		}

		public static void NormalizeVector(ref Vector3 v1) {
			float length = (float) Math.Sqrt((v1.X * v1.X) + (v1.Y * v1.Y) + (v1.Z * v1.Z));
			v1.X = v1.X / length;
			v1.Y = v1.Y / length;
			v1.Z = v1.Z / length;
		}

		public static Vector3 CentroidFromVertices(in List<Vector3> vertices) {
			float sumX = 0;
			float sumY = 0;
			float sumZ = 0;

			for (int i = 0; i < vertices.Count; i++) {
				sumX += vertices[i].X;
				sumY += vertices[i].Y;
				sumZ += vertices[i].Z;
			}

			return new Vector3(sumX / vertices.Count, sumY / vertices.Count, sumZ / vertices.Count);
		}
	}
}
