# README

## Why Component Names?

Ever had trouble distinguishing multiple colliders on the same GameObject from each other?

Ever wished you could rename that generic "Event Trigger" component to something a little more descriptive like "On Click Open Settings"?

And what if you could attach little notes to your components to provide some clarifying context?

Wouldn't it be great if you could see the current state of that "Health" component right in the title without having to unfold it?

With Component Names you can!

Component Names integrates seamlessly with the Inspector and makes it possible to rename components at will as if it was a native feature.

## Features
* Select a component's header and press F2 to start renaming it (or select "Rename" from the context menu if your keyboard is broken).
* Default name is shown in parentheses after the custom name by default.
* Add a custom suffix after the default name by typing "(suffix)".
* Give a custom name and suffix by typing "Name (suffix)".
* Give a custom name without default name suffix by typing "Name ()".
* Custom component names are shown in Object fields and UnityEvent fields.
* All custom name data is fully stripped from builds.

## Features For Coders
* Extension methods component.GetName() and component.SetName(string) make it simple to get or set component names in code.
* Easily change Component.name and Component.ToString() to return the component's name by deriving from a custom base class or by overriding them yourself.
* Dynamically change the Component's name based on its current state via OnValidate.