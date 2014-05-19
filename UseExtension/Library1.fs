module UseExtension

open ExtensionMethodTypeProvider
open ExtensionProvider


let test () =
    let i = 23
    i.FromString("32") |> ignore
    i.AnotherFromString("32") |> ignore
    ExtensionProvider.Int32Extension.FromString(23, "32")
    
