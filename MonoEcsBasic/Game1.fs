namespace MonoGameEcsTest

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Garnet.Composition
open MonoGameEcsTest.Systems.DrawSystem
open MonoGameEcsTest.Systems.KeyboardInputSystem
open MonoGameEcsTest.Systems.InputSystem
open MonoGameEcsTest.Systems.CollisionSystem

type Game1() as this =
    inherit Game()

    let graphics = new GraphicsDeviceManager(this)

    let world = Container()

    let mutable spriteBatch = Unchecked.defaultof<_>
    let mutable defaultTexture = Unchecked.defaultof<_>
    let screenSize = Vector2(1280f, 720f)

    override this.Initialize() =
        this.IsMouseVisible <- true
        graphics.PreferredBackBufferWidth <- screenSize.X |> int
        graphics.PreferredBackBufferHeight <- screenSize.Y |> int
        graphics.ApplyChanges()

        base.Initialize()

    override this.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        defaultTexture <- new Texture2D(this.GraphicsDevice, 1, 1)
        defaultTexture.SetData [| Color.White |]

        registerKeyboardInput world
        registerInput world

        registerCollision world

        registerDraw world

        Paddle.registerPlayerMovement world screenSize
        Ball.registerBallMovement world screenSize

        Paddle.create 30f Kb1 world
        Paddle.create (screenSize.X - 50f) Kb2 world

        Ball.create world screenSize

        base.LoadContent()

    override _.Update gameTime =
        world.Run { UpdateTime = gameTime }

        base.Update gameTime

    override this.Draw gameTime =
        this.GraphicsDevice.Clear Color.Black

        spriteBatch.Begin()

        world.Run { 
            DrawTime = gameTime
            SpriteBatch = spriteBatch
            DefaultTexture = defaultTexture
        }

        spriteBatch.End()

        base.Draw gameTime

