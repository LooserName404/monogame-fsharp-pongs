namespace MonoGameDesktopApp

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open System

type Game1() as this =
    inherit Game()

    do
        this.Content.RootDirectory <- "Content"
        this.IsFixedTimeStep <- true
        this.IsMouseVisible <- true
    
    let graphics = new GraphicsDeviceManager(this)
    do graphics.IsFullScreen <- false

    let mutable spriteBatch = Unchecked.defaultof<_>
    let mutable defaultTexture = Unchecked.defaultof<_>
    let mutable screenSize = Unchecked.defaultof<_>

    let random = Random()

    let pickRandomDirection() = random.Next(3, 7) * (if random.Next(2) = 0 then 1 else -1) |> float32

    let pad1 = { Position = Vector2(30f, 10f); Input = WASD; Width = 20; Height = 80 }
    let pad2 = { Position = Vector2(750f, 10f); Input = ArrowKeys; Width = 20; Height = 80 }
    let ball = { 
        Position = Vector2(200f, 200f)
        Velocity = Vector2(pickRandomDirection(), pickRandomDirection())
                
        Size = 20 
    }

    override _.Initialize() =
        graphics.IsFullScreen <- false
        graphics.PreferredBackBufferWidth <- 800
        graphics.PreferredBackBufferHeight <- 600
        graphics.ApplyChanges()

        screenSize <- Vector2(800f, 600f)

        base.Initialize()

    override _.LoadContent() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)

        defaultTexture <- new Texture2D(this.GraphicsDevice, 1, 1)
        defaultTexture.SetData [| Color.White |]

        base.LoadContent()

    override _.UnloadContent() = base.UnloadContent()

    override _.Update (gameTime: GameTime) = 
        pad1.Update(Keyboard.GetState())
        pad2.Update(Keyboard.GetState())

        ball.Update screenSize [pad1; pad2]

        base.Update gameTime

    override _.Draw (gameTime: GameTime) = 
        this.GraphicsDevice.SetRenderTarget(null)
        this.GraphicsDevice.Clear Color.CornflowerBlue

        spriteBatch.Begin()

        pad1.Draw spriteBatch defaultTexture
        pad2.Draw spriteBatch defaultTexture
        ball.Draw spriteBatch defaultTexture

        spriteBatch.End()

        base.Draw gameTime

