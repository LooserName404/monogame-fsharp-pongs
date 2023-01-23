namespace MonoGameDesktopApp

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input


[<AutoOpen>]
module private Paddle =
    [<Literal>]
    let paddleSpeed = 8

type Paddle = {
    mutable Position: Vector2 
    Input: InputDevice
    Width: int
    Height: int
}
with
    member this.Box = Rectangle(this.Position.X |> int, this.Position.Y |> int, this.Width, this.Height)

    member this.Draw (spriteBatch: SpriteBatch) (texture: Texture2D) =
        spriteBatch.Draw(texture, this.Box, Color.White)

    member this.Update (keyboard: KeyboardState) =
        if InputManager.up this.Input keyboard then
            this.Position <- Vector2(this.Position.X, this.Position.Y - (paddleSpeed |> float32))
        elif InputManager.down this.Input keyboard then
            this.Position <- Vector2(this.Position.X, this.Position.Y + (paddleSpeed |> float32))
