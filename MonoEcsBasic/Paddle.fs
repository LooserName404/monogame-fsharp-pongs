namespace MonoGameEcsTest

open Garnet.Composition
open Microsoft.Xna.Framework

[<RequireQualifiedAccess>]
module Paddle =

    let registerPlayerMovement (container: Container) (screenSize: Vector2) =
        container.On(
            fun (cmd: InputActionCommand) struct(Position position, InputListener inputListener, Speed speed, _: Player) ->
                if InputListener.isTargetDeviceAndInput cmd.Device inputListener.Device cmd.Input inputListener.Inputs then
                    match cmd.Input with
                    | InputUp -> Position {| position with Y = position.Y - speed |}
                    | InputDown -> Position {| position with Y = position.Y + speed |}
                else Position position
            |> Join.update4
            |> Join.over container
        )
        |> ignore

        container.On(
            fun (_: UpdateCommand) struct(Position position, Size size, _: Player) ->
                let height =
                    match size with
                    | RectSize r -> r.Height
                    | SquareSize s -> s

                if position.Y < 0f then
                    Position {| position with Y = 0f |}
                elif position.Y + height > screenSize.Y then
                    Position {| position with Y = screenSize.Y - height |}
                else Position position
            |> Join.update3
            |> Join.over container
        )
        |> ignore
    
    let create initialPosition device (container: Container) = 
        container.Create()
            .With(Player())
            .With(Position {| X = initialPosition; Y = 0f |})
            .With(Speed 7f)
            .With(Size (RectSize {| Width = 20f; Height = 80f |}))
            .With(Renderer DefaultTexture)
            .With(InputListener {| Inputs = [| InputUp; InputDown |]; Device = device |})
            .With(Collider [| "Paddle" |])
        |> ignore

