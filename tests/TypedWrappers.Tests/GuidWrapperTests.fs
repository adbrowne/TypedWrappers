module GuidWrapper.Tests

open System
open TypedWrappers
open Xunit

type WidgetId = GuidWrapper<"bc4bccca-fb2d-48bf-a328-ece4eefaa517">
type WidgetIdDuplicate = GuidWrapper<"bc4bccca-fb2d-48bf-a328-ece4eefaa517">
type FooId = GuidWrapper<"c38b4c03-b055-4884-82c5-83d6cc2505aa">

[<Fact>]
let ``WidgetIds with the same Guid are equal`` () =
  let guid = Guid.NewGuid()
  let myId = new WidgetId(guid)
  let myId2 = new WidgetId(guid)

  Assert.Equal(myId, myId2)

[<Fact>]
let ``WidgetIds with different Guids are not equal`` () =
  let myId = new WidgetId(Guid.NewGuid())
  let myId2 = new WidgetId(Guid.NewGuid())

  Assert.NotEqual(myId, myId2)

[<Fact>]
let ``WidgetIds are never equal to FooIds`` () =
  let guid = Guid.NewGuid()
  let myId = new WidgetId(guid) :> obj
  let myId2 = new FooId(guid) :> obj

  Assert.NotEqual(myId, myId2)

// I would love to remove the caching here but don't know how
[<Fact>]
let ``Types with the same Guid are equal`` () =
  let guid = Guid.NewGuid()
  let myId = new WidgetId(guid)
  let myId2 = new FooId(guid)

  Assert.Equal(typeof<WidgetId>, typeof<WidgetIdDuplicate>)