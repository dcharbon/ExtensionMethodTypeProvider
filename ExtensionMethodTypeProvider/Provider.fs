module ExtensionMethodTypeProvider

open Samples.FSharp.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open System.Reflection
open System.Runtime.CompilerServices

[<TypeProvider>]
type public ExtensionMethodProvider(cfg:TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()

    let runtimeAssembly = Assembly.ReflectionOnlyLoadFrom cfg.RuntimeAssembly

    let buildTypes () =
        let ns = "ExtensionProvider"
        let asm = runtimeAssembly

        // Try to define an extension method for string
        let extension = ProvidedTypeDefinition(runtimeAssembly, ns, "Int32Extension", Some typeof<obj>, ExtensionAttribute = true)
        let pmethod =
            ProvidedMethod(
                methodName = "FromString",
                parameters = [
                    ProvidedParameter("thisptr", typeof<System.Int32>);
                    ProvidedParameter("arg", typeof<string>)
                    ],
                returnType = typeof<int>,
                IsStaticMethod = true,
                ExtensionAttribute = true,
                InvokeCode = fun args ->
                    <@@ System.Int32.Parse(%%args.[1]) @@>
                )
        extension.AddMember(pmethod)

        this.AddNamespace(ns, [extension])

    do buildTypes()

[<ExtensionAttribute>]
type AnotherExtension =
    [<ExtensionAttribute>]
    static member AnotherFromString(self: System.Int32, arg: string) =
        System.Int32.Parse(arg)

[<TypeProviderAssembly>]
do()
