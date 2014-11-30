namespace TypedWrappers

open System
open System.Reflection
open Samples.FSharp.ProvidedTypes
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations

// This type defines the type provider. When compiled to a DLL, it can be added
// as a reference to an F# command-line compilation, script, or project.
[<TypeProvider>]
type SampleTypeProvider(config: TypeProviderConfig) as this = 

    // Inheriting from this type provides implementations of ITypeProvider 
    // in terms of the provided types below.
    inherit TypeProviderForNamespaces()

    let namespaceName = "Samples.HelloWorldTypeProvider"
    let thisAssembly = Assembly.GetExecutingAssembly()

    // Make one provided type, called TypeN.
    let makeOneProvidedType (n:int) = 
      // This is the provided type. It is an erased provided type and, in compiled code, 
      // will appear as type 'obj'.
      let t = ProvidedTypeDefinition(thisAssembly,namespaceName,
                                       "Type" + string n,
                                       baseType = Some typeof<obj>)

      let staticProp = ProvidedProperty(propertyName = "StaticProperty", 
                                  propertyType = typeof<string>, 
                                  IsStatic=true,
                                  GetterCode= (fun args -> <@@ "Hello!" @@>))
      staticProp.AddXmlDocDelayed(fun () -> "This is a static property")
      t.AddMember staticProp
      let ctor = ProvidedConstructor(parameters = [ ], 
                                     InvokeCode= (fun args -> <@@ "The object data" :> obj @@>))
      ctor.AddXmlDocDelayed(fun () -> "This is a constructor")

      t.AddMember ctor

      let ctor2 = 
          ProvidedConstructor(parameters = [ ProvidedParameter("data",typeof<string>) ], 
                        InvokeCode= (fun args -> <@@ (%%(args.[0]) : string) :> obj @@>))
      //t.AddMember ctor

      let instanceMeth = 
          ProvidedMethod(methodName = "InstanceMethod", 
                   parameters = [ProvidedParameter("x",typeof<int>)], 
                   returnType = typeof<char>, 
                   InvokeCode = (fun args -> 
                       <@@ ((%%(args.[0]) : obj) :?> string).Chars(%%(args.[1]) : int) @@>))

      instanceMeth.AddXmlDocDelayed(fun () -> "This is an instance method")
      // Add the instance method to the type.
      t.AddMember instanceMeth

      // The result of makeOneProvidedType is the type.
      t

    // Now generate 100 types
    let types = [ for i in 1 .. 100 -> makeOneProvidedType i ] 

    // And add them to the namespace
    do this.AddNamespace(namespaceName, types)

[<assembly:TypeProviderAssembly>] 
do()
