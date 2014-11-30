namespace TypedWrappers

open System

open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open Samples.FSharp.ProvidedTypes

[<TypeProvider>]
type public GuidWrapper() as this =
    inherit TypeProviderForNamespaces()

    // Get the assembly and namespace used to house the provided types.
    let thisAssembly = Assembly.GetExecutingAssembly()
    let rootNamespace = "TypedWrappers"
    let baseTy = typeof<obj>
    let staticParams = [ProvidedStaticParameter("ignored", typeof<string>)]
    let guidTy = ProvidedTypeDefinition(thisAssembly, rootNamespace, "GuidWrapper", Some baseTy)

    do guidTy.DefineStaticParameters(
        staticParameters=staticParams, 
        apply=(fun typeName parameters ->
        let t = ProvidedTypeDefinition(thisAssembly, rootNamespace, typeName, Some baseTy)

        let ctor = 
              ProvidedConstructor(parameters = [ ProvidedParameter("guid",typeof<Guid>) ], 
                            InvokeCode= (fun args -> <@@ (%%(args.[0]) : Guid) :> obj @@>))
        t.AddMember ctor

        t))

    do this.AddNamespace(rootNamespace, [guidTy])

[<TypeProviderAssembly>]
do ()