namespace MonoGameDesktopApp

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type Ball = {
    mutable Position: Vector2
    mutable Velocity: Vector2
    Size: int
}
with
    member this.Box = Rectangle(this.Position.X |> int, this.Position.Y |> int, this.Size, this.Size)

    member this.Draw (spriteBatch : SpriteBatch) (texture: Texture2D) =
        spriteBatch.Draw(texture, this.Box, Color.White)

    member this.Update (screenSize : Vector2) (paddles: Paddle seq) =
        let oldPos = this.Position
        this.Position <- Vector2(this.Position.X + this.Velocity.X, this.Position.Y + this.Velocity.Y)

        let intersects =
            paddles
            |> Seq.exists (fun p -> this.Box.Intersects p.Box)

        if this.Position.X < 0f || this.Position.X + (float32 this.Size) > screenSize.X || intersects then
            this.Velocity <- Vector2(this.Velocity.X * -1f, this.Velocity.Y)
            this.Position <- oldPos

        if this.Position.Y < 0f || (this.Position.Y + (float32 this.Size)) > screenSize.Y then
            this.Velocity <- Vector2(this.Velocity.X, this.Velocity.Y * -1f)
            this.Position <- oldPos
        
        ()
