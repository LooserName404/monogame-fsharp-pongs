open MonoGameDesktopApp

[<EntryPoint>]
let main args =
    use game = new Game1()

    game.Run()

    0
