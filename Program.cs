using System;

using Raylib_cs;
using static Raylib_cs.CameraType;
using static Raylib_cs.CameraMode;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Color;
using System.Numerics;

namespace VectorField3D {

	class Program {
		static void Main(string[] args) {
            const int screenWidth = 1920;
            const int screenHeight = 1080;

            Raylib.SetConfigFlags(ConfigFlag.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(screenWidth, screenHeight, "Vector Field 3D");

            float cubeSize = 128f;

            Camera3D camera;
            camera.position = new Vector3(cubeSize, cubeSize, cubeSize);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 60f;
            camera.type = CAMERA_PERSPECTIVE;
            Raylib.SetCameraMode(camera, CAMERA_FREE);

            VectorField vf = new VectorField((int) Math.Pow(2, 14), (int) Math.Pow(2, 7), cubeSize);

            while (!Raylib.WindowShouldClose()) {
	            float deltaTime = Raylib.GetFrameTime();

                Raylib.UpdateCamera(ref camera);
                if (Raylib.IsKeyDown(KEY_Z)) camera.target = new Vector3(0.0f, 0.0f, 0.0f);

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                Raylib.BeginMode3D(camera);

                vf.Update(deltaTime);
                vf.Draw();

                Raylib.DrawCubeWires(new Vector3(0f, 0f, 0f), cubeSize, cubeSize, cubeSize, Color.WHITE);

                Raylib.EndMode3D();

                Raylib.DrawFPS(10, 10);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
		}
	}
}
