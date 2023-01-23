namespace MonoGameDesktopApp

open Microsoft.Xna.Framework.Input

type InputDevice = | WASD | ArrowKeys

module InputManager =

    let up device (source: KeyboardState) =
        match device with
        | WASD -> source.IsKeyDown Keys.W
        | ArrowKeys -> source.IsKeyDown Keys.Up

    let down device (source: KeyboardState) =
        match device with
        | WASD -> source.IsKeyDown Keys.S
        | ArrowKeys -> source.IsKeyDown Keys.Down
