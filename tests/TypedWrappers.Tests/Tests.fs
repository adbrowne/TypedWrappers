module TypedWrappers.Tests

open TypedWrappers
open NUnit.Framework
open Samples.FSharp.RegexTypeProvider

[<Test>]
let ``hello returns 42`` () =
  let result = Library.hello 42
  printfn "%i" result
  Assert.AreEqual(42,result)

[<Test>]
let ``can use type provider type`` () =
  let foo = new Samples.HelloWorldTypeProvider.Type1()
  printfn "%s" Samples.HelloWorldTypeProvider.Type1.StaticProperty

type T = Samples.FSharp.RegexTypeProvider.RegexTyped< @"(?<AreaCode>^\d{3})-(?<PhoneNumber>\d{3}-\d{4}$)">
    
[<Test>]
let ``regex type provider`` () =
    let reg = T()
    let result = T.IsMatch("425-555-2345")
    //let r = reg.Match("425-555-2345"). //  .Group_AreaCode.Value //r equals "425"
    Assert.True(true)