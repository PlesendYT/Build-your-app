using Stride.Engine;
using Stride.Input;
using Stride.Physics;
using Stride.Core.Mathematics;

namespace GameStride;

public class GameScript : AsyncScript
{
    public override async Task Execute()
    {
        var player = Entity;
        var startPos = player.Transform.Position;
        float speed = 5f;

        while (Game.IsRunning)
        {
            await Script.NextFrame();

            var input = Input;
            var move = Vector3.Zero;

            if (input.IsKeyDown(Keys.Left)) move.X -= speed;
            if (input.IsKeyDown(Keys.Right)) move.X += speed;
            if (input.IsKeyDown(Keys.Up)) move.Z -= speed;
            if (input.IsKeyDown(Keys.Down)) move.Z += speed;

            player.Transform.Position += move * (float)Game.UpdateTime.WaitElapsed.TotalSeconds;
        }
    }
}
