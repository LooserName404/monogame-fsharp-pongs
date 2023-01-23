namespace MonoGameEcsTest.Systems

open Garnet.Composition
open MonoGameEcsTest

module KeyboardInputSystem =
    open Microsoft.Xna.Framework.Input

    let private convertKeysToInputs (keys: Keys array) =
        keys
        |> Array.choose (
            fun k -> 
                match k with
                | Keys.W -> Some (InputUp, Kb1)
                | Keys.S -> Some (InputDown, Kb1)
                | Keys.Up -> Some (InputUp, Kb2)
                | Keys.Down -> Some (InputDown, Kb2)
                | _ -> None)

    let registerKeyboardInput (container: Container) =
        container.On(
            fun (_: UpdateCommand) ->
                let keyboard = Keyboard.GetState()

                let pressedKeys = keyboard.GetPressedKeyCount()

                if pressedKeys > 0 then
                    let inputs = keyboard.GetPressedKeys() |> convertKeysToInputs

                    container.Run { Inputs = inputs }
                ()
        )
        |> ignore

