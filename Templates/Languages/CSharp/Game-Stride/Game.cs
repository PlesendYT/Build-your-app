using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using Stride.Core.Mathematics;

namespace GameStride;

/// <summary>Async script that moves the entity based on arrow-key input each frame.</summary>
public class GameScript : AsyncScript
{
    /// <summary>Main game loop: reads input and updates position every frame.</summary>
    public override async Task Execute()
    {
        var player = Entity;
        var startPos = player.Transform.Position;
        float speed = 5f;

        // Loop while the game is running
        while (Game.IsRunning)
        {
            await Script.NextFrame();

            // Accumulate movement vector from arrow keys
            var input = Input;
            var move = Vector3.Zero;

            if (input.IsKeyDown(Keys.Left)) move.X -= speed;
            if (input.IsKeyDown(Keys.Right)) move.X += speed;
            if (input.IsKeyDown(Keys.Up)) move.Z -= speed;
            if (input.IsKeyDown(Keys.Down)) move.Z += speed;

            // Apply movement scaled by frame time
            player.Transform.Position += move * (float)Game.UpdateTime.WaitElapsed.TotalSeconds;
        }
    }
}
