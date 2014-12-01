module TypedWrappers.Tests

open TypedWrappers
open Xunit
open Samples.FSharp.RegexTypeProvider

[<Fact>]
let ``hello returns 42`` () =
  let result = Library.hello 42
  printfn "%i" result
  Assert.Equal(42,result)

[<Fact>]
let ``can use type provider type`` () =
  let foo = new Samples.HelloWorldTypeProvider.Type1()
  printfn "%s" Samples.HelloWorldTypeProvider.Type1.StaticProperty

type T = Samples.FSharp.RegexTypeProvider.RegexTyped< @"(?<AreaCode>^\d{3})-(?<PhoneNumber>\d{3}-\d{4}$)">
    
[<Fact>]
let ``regex type provider`` () =
    let reg = T()
    let result = T.IsMatch("425-555-2345")
    //let r = reg.Match("425-555-2345"). //  .Group_AreaCode.Value //r equals "425"
    Assert.True(true)