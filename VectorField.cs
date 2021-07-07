using System;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;

namespace VectorField3D {

	public struct Point {
		public Vector3 Position;
		public Vector3 Heading;
		public float Velocity;
		public Color Color;

		public Point(Vector3 pos, Vector3 head, float vel, Color color) {
			this.Position = pos;
			this.Heading = head;
			this.Velocity = vel;
			this.Color = color;
		}
	}

	public struct Field {
		public Vector3 Heading;
		public float Velocity;

		public Field(Vector3 head, float vel) {
			this.Heading = head;
			this.Velocity = vel;
		}
	}

	public class VectorField {
		public int Point1DCount;
		public int PointCount;
		public int Field1DCount;
		public int FieldCount;

		public float CubeSize;
		public Point[,,] Points;
		public Field[,,] Fields;

		public VectorField(int pointCount, int fieldCount, float cubeSize) {
			this.PointCount = pointCount;
			this.Point1DCount = (int) Math.Round(Math.Pow(this.PointCount, 1f / 3f));
			this.FieldCount = fieldCount;
			this.Field1DCount = (int) Math.Round(Math.Pow(this.FieldCount, 1f / 3f));
			this.CubeSize = cubeSize;

			this.Init();
		}

		public Point CreatePoint() {
			Vector3 pos = new Vector3(Helpers.RandomH.GetRandom(-this.CubeSize / 2f, this.CubeSize / 2f), Helpers.RandomH.GetRandom(-this.CubeSize / 2f, this.CubeSize / 2f), Helpers.RandomH.GetRandom(-this.CubeSize / 2f, this.CubeSize / 2f));
			Vector3 head = Helpers.Vector.NormalizeVector(new Vector3(Helpers.RandomH.GetRandom(-1f, 1f),Helpers.RandomH.GetRandom(-1f, 1f),Helpers.RandomH.GetRandom(-1f, 1f)));
			float vel = Helpers.RandomH.GetRandom(0f, this.CubeSize);
			Color color = Helpers.RandomH.GetRandomColor();

			return new Point(pos, head, vel, color);
		}

		public Field CreateField() {
			Vector3 head = Helpers.Vector.NormalizeVector(new Vector3(Helpers.RandomH.GetRandom(-1f, 1f),Helpers.RandomH.GetRandom(-1f, 1f),Helpers.RandomH.GetRandom(-1f, 1f)));
			float vel = Helpers.RandomH.GetRandom(0f, this.CubeSize);

			return new Field(head, vel);
		}

		public void Init() {
			this.Points = new Point[this.Point1DCount,this.Point1DCount,this.Point1DCount];
			this.Fields = new Field[this.Field1DCount,this.Field1DCount,this.Field1DCount];

			for (int i = 0; i < this.Point1DCount; i++) {
				for (int j = 0; j < this.Point1DCount; j++) {
					for (int k = 0; k < this.Point1DCount; k++) {
						this.Points[i, j, k] = this.CreatePoint();
					}
				}
			}

			for (int i = 0; i < this.Field1DCount; i++) {
				for (int j = 0; j < this.Field1DCount; j++) {
					for (int k = 0; k < this.Field1DCount; k++) {
						this.Fields[i, j, k] = this.CreateField();
					}
				}
			}
		}

		public void Update(float deltaTime) {
			Parallel.For((int) 0, this.Point1DCount, i => {
				for (int j = 0; j < this.Point1DCount; j++) {
					for (int k = 0; k < this.Point1DCount; k++) {

						int fi = (int) Helpers.TweenH.Linear(this.Points[i, j, k].Position.X, -this.CubeSize / 2f, this.CubeSize / 2f, 0, this.Field1DCount - 1);
						int fj = (int) Helpers.TweenH.Linear(this.Points[i, j, k].Position.Y, -this.CubeSize / 2f, this.CubeSize / 2f, 0, this.Field1DCount - 1);
						int fk = (int) Helpers.TweenH.Linear(this.Points[i, j, k].Position.Z, -this.CubeSize / 2f, this.CubeSize / 2f, 0, this.Field1DCount - 1);

						this.Fields[fi, fj, fk].Heading += ((this.Points[i, j, k].Heading) * 0.002f);
						Helpers.Vector.NormalizeVector(ref this.Fields[fi, fj, fk].Heading);
						this.Fields[fi, fj, fk].Velocity += ((this.Points[i, j, k].Velocity - this.Fields[fi, fj, fk].Velocity) * 0.002f);

						this.Points[i, j, k].Heading += ((this.Fields[fi, fj, fk].Heading) * 0.15f);
						Helpers.Vector.NormalizeVector(ref this.Points[i, j, k].Heading);
						this.Points[i, j, k].Velocity += ((this.Fields[fi, fj, fk].Velocity - this.Points[i, j, k].Velocity) * 0.15f);

						this.Points[i, j, k].Position += ((this.Points[i, j, k].Heading * this.Points[i, j, k].Velocity) * deltaTime);

						this.Points[i, j, k].Position.X = Helpers.MathH.ModRange(this.Points[i, j, k].Position.X, -this.CubeSize / 2f, this.CubeSize / 2f);
						this.Points[i, j, k].Position.Y = Helpers.MathH.ModRange(this.Points[i, j, k].Position.Y, -this.CubeSize / 2f, this.CubeSize / 2f);
						this.Points[i, j, k].Position.Z = Helpers.MathH.ModRange(this.Points[i, j, k].Position.Z, -this.CubeSize / 2f, this.CubeSize / 2f);

						Helpers.TweenH.SmoothToTarget(ref this.Points[i, j, k].Velocity, 32f, 1024f);
						Helpers.TweenH.SmoothToTarget(ref this.Fields[fi, fj, fk].Velocity, 32f, 1024f);

						this.Points[i, j, k].Color = new Color((byte) Helpers.TweenH.Linear(this.Points[i,j,k].Heading.X, -1f, 1f, 0, 255), (byte)Helpers.TweenH.Linear(this.Points[i,j,k].Heading.Y, -1f, 1f, 0, 255), (byte)Helpers.TweenH.Linear(this.Points[i,j,k].Heading.Z, -1f, 1f, 0, 255), (byte)255);

						if (Helpers.RandomH.GetRandom(0f, 1f) < 0.005f) {
							this.Points[i, j, k] = this.CreatePoint();
						}

						if (Helpers.RandomH.GetRandom(0f, 1f) < 0.0005f) {
							this.Fields[fi, fj, fk] = this.CreateField();
						}
					}
				}
			});
		}

		public void Draw() {
			for (int i = 0; i < this.Point1DCount; i++) {
				for (int j = 0; j < this.Point1DCount; j++) {
					for (int k = 0; k < this.Point1DCount; k++) {

						//Raylib.DrawPoint3D(this.Points[i,j,k].Position, this.Points[i,j,k].Color);
						Raylib.DrawCube(this.Points[i,j,k].Position, 0.5f, 0.5f, 0.5f, this.Points[i,j,k].Color);
						//Raylib.DrawSphere(this.Points[i,j,k].Position, 0.5f, this.Points[i,j,k].Color);
						//Raylib.DrawLine3D(this.Points[i,j,k].Position, this.Points[i,j,k].Position + (this.Points[i,j,k].Heading * 0.001f), this.Points[i,j,k].Color);
					}
				}
			}
		}
	}
}
