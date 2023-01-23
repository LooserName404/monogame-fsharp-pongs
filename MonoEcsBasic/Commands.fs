namespace MonoGameEcsTest

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Garnet.Composition

[<Struct>] type DrawCommand = { DrawTime: GameTime; SpriteBatch: SpriteBatch; DefaultTexture: Texture2D }
[<Struct>] type UpdateCommand = { UpdateTime: GameTime }
[<Struct>] type InputCommand = { Inputs: (Input * Device) array }
[<Struct>] type InputActionCommand = { Input: Input; Device: Device }
[<Struct>] type ChangeBallDirectionCommand = { ChangeX: bool; ChangeY: bool }
[<Struct>] type CollisionCommand = { Entity: Eid; Tag: string }
