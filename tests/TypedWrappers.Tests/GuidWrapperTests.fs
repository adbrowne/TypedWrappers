module GuidWrapper.Tests

open System
open TypedWrappers
open NUnit.Framework

type WidgetId = GuidWrapper<"Nothing">
type FooId = GuidWrapper<"Nothing">

[<Test>]
let ``WidgetIds with the same Guid are equal`` () =
  let guid = Guid.NewGuid()
  let myId = new WidgetId(guid)
  let myId2 = new WidgetId(guid)

  Assert.AreEqual(myId, myId2)

[<Test>]
let ``WidgetIds with different Guids are not equal`` () =
  let myId = new WidgetId(Guid.NewGuid())
  let myId2 = new WidgetId(Guid.NewGuid())

  Assert.AreNotEqual(myId, myId2)