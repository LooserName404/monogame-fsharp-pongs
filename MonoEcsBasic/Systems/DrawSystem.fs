namespace MonoGameEcsTest.Systems

open Microsoft.Xna.Framework
open Garnet.Composition
open MonoGameEcsTest

module DrawSystem =

    let registerDraw (container: Container) =
        container.On<DrawCommand> (
            fun (draw: DrawCommand) struct ((Position position), (Size size), (Renderer renderer)) ->
                let sb = draw.SpriteBatch

                let texture =
                    match renderer with
                    | DefaultTexture -> draw.DefaultTexture
                    | CustomTexture t -> t

                let w, h =
                    match size with
                    | SquareSize x -> int x, int x
                    | RectSize r -> int r.Width, int r.Height

                let rect = Rectangle(position.X |> int, position.Y |> int , w, h)

                sb.Draw(texture, rect, Color.White)

                ()
            |> Join.iter3
            |> Join.over container
        ) |> ignore

