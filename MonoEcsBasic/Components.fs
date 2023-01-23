namespace MonoGameEcsTest

open Microsoft.Xna.Framework.Graphics

[<Struct>] type Player = struct end

[<Struct>] type BallMarker = struct end

[<Struct>] type TextureType = | DefaultTexture | CustomTexture of Texture2D

[<Struct>] type Renderer = Renderer of TextureType

[<Struct>] type Position = Position of {| X: float32; Y: float32 |}

[<Struct>] type SizeForm = SquareSize of size: float32 | RectSize of {| Width: float32; Height: float32 |}

[<Struct>] type Size = Size of SizeForm

[<Struct>] type Speed = Speed of float32

[<Struct>] type Direction = Direction of {| X: float32; Y: float32 |}

[<Struct>] type Collider = Collider of Tags: string array

[<Struct>] type InputListener = InputListener of {| Inputs: Input array; Device: Device |}

[<Struct>] type CollisionListener = CollisionListener of Tags: string array

[<RequireQualifiedAccess>]
module InputListener =

    let (|TargetDevice|_|) registeredDevice device =
        if device = registeredDevice then Some device
        else None

    let (|TargetInput|_|) registeredInputs input =
        if registeredInputs |> Array.contains input then Some input
        else None

    let isTargetDeviceAndInput device targetDevice input targetInput =
        match device, input with
        | TargetDevice targetDevice _, TargetInput targetInput _ -> true
        | _ -> false
