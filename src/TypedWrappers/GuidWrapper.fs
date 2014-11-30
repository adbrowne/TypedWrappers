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
    let staticParams = [ProvidedStaticParameter("UniqueTypeIdentity", typeof<string>)]
    let guidTy = ProvidedTypeDefinition(thisAssembly, rootNamespace, "GuidWrapper", Some baseTy)

    let currentTypeIndex = ref -1
    do guidTy.DefineStaticParameters(
        staticParameters=staticParams, 
        apply=(fun typeName parameterValues ->
        
        match parameterValues with 
        | [| :? string as typeIdString|] -> 
            let (parsed,typeId) = Guid.TryParse(typeIdString)

            if not parsed then
                failwith "UniqueTypeIdentity must be a guid: %a" typeIdString
               
            let typeIndex = System.Threading.Interlocked.Increment currentTypeIndex
            let t = ProvidedTypeDefinition(thisAssembly, rootNamespace, typeName, Some baseTy)

            let ctor = 
                  ProvidedConstructor(parameters = [ ProvidedParameter("guid",typeof<Guid>) ], 
                                InvokeCode= (fun args -> <@@ ((typeIndex, %%(args.[0])) : (int * Guid)) :> obj @@>))
            t.AddMember ctor

            t
        | _ -> failwith "unexpected parameter values")) 

    do this.AddNamespace(rootNamespace, [guidTy])

[<TypeProviderAssembly>]
do ()