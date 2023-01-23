namespace MonoGameEcsTest.Systems

open Garnet.Composition
open MonoGameEcsTest

module CollisionSystem =

    let registerCollision (container: Container) = 
        container.On(
            fun (_: UpdateCommand) ->
                let colliders = container.Query<Eid, Position, Size, Collider>()
                let getSize (Size size) =
                    match size with
                    | RectSize r -> {| Width = r.Width; Height = r.Height |}
                    | SquareSize s -> {| Width = s; Height = s |}
                colliders
                |> Seq.take (colliders.GetCount() - 1)
                |> Seq.iteri (fun index collider ->
                    let nextItems = colliders |> Seq.skip (index + 1)
                    let e1 = collider.Value1
                    let (Position p1) = collider.Value2
                    let s1 = getSize collider.Value3
                    let (Collider c1) = collider.Value4

                    nextItems
                    |> Seq.iter (fun next ->
                        let e2 = next.Value1
                        let (Position p2) = next.Value2
                        let s2 = getSize next.Value3
                        let (Collider c2) = next.Value4

                        if p1.X < p2.X + s2.Width &&
                            p1.X + s1.Width > p2.X &&
                            p1.Y < p2.Y + s2.Height &&
                            p1.Y + s1.Height > p2.Y then
                            let entity1 = container.Get e1
                            let entity2 = container.Get e2
                            let sendEachTagIfHasListener (entity: Entity) targetTags =
                                if entity.Has<CollisionListener>() then
                                    let eid = entity.Id
                                    let (CollisionListener l) = entity.Get<CollisionListener>()
                                    let listened = l |> Array.filter (fun x -> targetTags |> Array.contains x)
                                    listened |> Array.iter (fun i -> container.Run { Entity = eid; Tag = i })
                            sendEachTagIfHasListener entity1 c2
                            sendEachTagIfHasListener entity2 c1
                        )
                    ())
        )
        |> ignore

