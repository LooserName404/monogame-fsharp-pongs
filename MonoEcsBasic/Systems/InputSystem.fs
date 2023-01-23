namespace MonoGameEcsTest.Systems

open Garnet.Composition
open MonoGameEcsTest

module InputSystem =

    let registerInput (container: Container) =
        container.On<InputCommand> (
            fun input ->
                input.Inputs
                |> Array.iter (fun (input, device) ->
                    container.Run { Input = input; Device = device })
        )
        |> ignore

