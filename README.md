# Dungen

## What is it?
*Dungen* is an infinite-runner casual game made for mobile, built on Unity. The game is very simple, with enemies to battle, obstacles to overcome, and loot to collect.

## Who made it?
*Dungen* was made by David Barsamian, a software engineering student

## Why was the project made?
I made *Dungen* to practice making games, specifically for mobile platforms. I've also never made an "infinite runner" style game, so I challenged myself to come up with the tech.

This is not to say that I made it *well*, rather that I made it. There are a number of cheap shortcuts I took because I just wanted a finished product. These include, but are not limited to:
* Physically moving the player throughout space instead of keeping them in place. This means that if someone plays for long enough, they will end up in the tens of thousands in one direction until the engine crashes. Not great but it technically works.
* Instantiating and destroying new blocks instead of object pooling. The levels are divided up into chunks that are randomly selected from prefabs and instantiated on the fly. This is great because I can quickly iterate on new content, but it's not great because it'll probably cause garbage collection issues.
