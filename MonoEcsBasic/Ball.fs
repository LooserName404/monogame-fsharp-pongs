namespace MonoGameEcsTest

open Garnet.Composition
open Microsoft.Xna.Framework
open System

[<RequireQualifiedAccess>]
module Ball =

    let registerBallMovement (container: Container) (screenSize: Vector2) =
        container.On(
            fun (_: UpdateCommand) struct(Position position, Direction direction, Speed speed, Size size, _: BallMarker) ->
                let width, height =
                    match size with
                    | RectSize r -> r.Width, r.Height
                    | SquareSize s -> s, s

                let x = position.X + (direction.X * speed)
                let y = position.Y + (direction.Y * speed)

                let outsideX = x < 0f || x + width > screenSize.X
                let outsideY = y < 0f || y + height > screenSize.Y

                let keepIfOutside oldValue newValue isOutside = if isOutside then oldValue else newValue

                if outsideX || outsideY then container.Run { ChangeX = outsideX; ChangeY = outsideY }

                Position {| position with X = keepIfOutside position.X x outsideX; Y = keepIfOutside position.Y y outsideY |}
            |> Join.update5
            |> Join.over container
        )
        |> ignore

        container.On(
            fun (cmd: ChangeBallDirectionCommand) struct(Direction direction, _: BallMarker) ->
                Direction 
                    {| direction with 
                        X = if cmd.ChangeX then -direction.X else direction.X
                        Y = if cmd.ChangeY then -direction.Y else direction.Y |}
            |> Join.update2
            |> Join.over container
        )
        |> ignore

        container.On(
            fun (cmd: CollisionCommand) (_: BallMarker) ->
                if cmd.Tag = "Paddle" then container.Run { ChangeX = true; ChangeY = false }
            |> Join.iter1
            |> Join.over container
        )
        |> ignore

    let create (container: Container) (screenSize: Vector2) =
        let random = Random()
        let getRandomDirection () =
            let signal = if random.Next(2) = 0 then 1 else -1
            random.Next(3, 8) * signal |> float32

        let direction = Vector2(getRandomDirection(), getRandomDirection())
        direction.Normalize()

        container.Create()
            .With(BallMarker())
            .With(Renderer DefaultTexture)
            .With(Size (SquareSize 20f))
            .With(Position {| X = screenSize.X / 2f - 10f; Y = screenSize.Y / 2f - 10f |})
            .With(Speed 7f)
            .With(Direction {| X = direction.X; Y = direction.Y |})
            .With(Collider [| "Ball" |])
            .With(CollisionListener [| "Paddle" |])
        |> ignore
