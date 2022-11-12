open Raven.Client.Documents

type Customer = {
    FirstName: string
    LastName: string
}

[<EntryPoint>]
let main argv =
    printfn "Hello RaveDB!"

    use store = new DocumentStore ()
    store.Urls <-  [|"http://10.0.75.1:8080"|]
    store.Database <- "RavenDB1"
    use store = store.Initialize ()
    
    use session = store.OpenSession ()
    let customer1 = { FirstName = "Guy"; LastName = "Montag" }
    session.Store customer1
    session.SaveChanges ()

    let customers = query {
        for customer in session.Query<Customer>() do
        select customer
    }

    customers |> Seq.iter (printf "%A")

    printfn "Done!"
    0