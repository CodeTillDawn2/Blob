  Blob

  A Unity game prototype blending stealth, Katamari Damacy-style growth, and platformer elements. You play as a gelatinous cube — a classic D&D monster — with one distinguishing trait: it can grow
  indefinitely if it consumes enough biological matter.

  The core loop is tension-based. While small, the blob sneaks, avoids confrontation, and picks off isolated targets. As it consumes more and grows larger, stealth becomes impossible and direct combat
  becomes inevitable. The blob can reshape itself across all three dimensions as long as its total volume stays consistent, allowing it to compress into a thin shape to slip through narrow gaps, then 
  spread back out to its full mass on the other side.

  A Windows build (Build/Blob.exe) is included in the repository.

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Table of Contents

   1. Concept
   2. Mechanics
   3. Architecture
   4. Third-Party Plugins
   5. Technology Stack
   6. Opening the Project

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Concept

  The player is a "special" gelatinous cube in a D&D-inspired world — one that has no upper bound on growth. Small enough at the start to pass unnoticed, it stalks prey, engulfs them, and digests them.
  Consumed biological matter increases its size. As the blob grows, its mass makes stealth increasingly impractical, forcing the player into direct conflict. The intended feel is Katamari's satisfying
  accumulation of size, constrained by the awareness and aggression of an increasingly hostile world.

  Planned elements include:

   - Stealth phase where the blob avoids detection
   - Transition point where size triggers active enemy aggression
   - Platformer traversal that uses the blob's reshaping ability (squeeze through narrow gaps, extend to reach platforms)
   - Growth as both a reward and a liability

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Mechanics

  Tentacles

  The blob can deploy tentacles to attack enemies. Tentacles have a four-state animation lifecycle — spawn, readied, smack, despawn — each driven by an Animator state machine. On contact, a tentacle applies
  a whap force to the target and deals damage. A TargetedByTentacleModifier prevents the same target from being hit multiple times simultaneously.

  Digestion

  When something is engulfed, it enters the blob's stomach. InContactWithBlobModifier adjusts the object's physics drag while it is touching the blob. InBlobsStomachModifier pins it inside the blob's volume.
  DigestNutritionModifier depletes it and feeds the blob. All three are component-based modifiers that fire as coroutines and remove themselves when their condition clears.

  Damage Types

  Three damage types are defined as ScriptableObject enums: acid, fire, and piercing. The DamageTypeLibrary singleton exposes them globally. The DealDamageModifier applies a typed damage burst to a target as
  a one-shot coroutine component.

  Volume-Consistent Reshaping

  The blob can stretch or compress along any axis, provided total volume is preserved. This enables puzzle traversal: squeeze tall-and-thin to fit through a narrow corridor, then relax back to a wider shape
  on the other side.

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Architecture

  The project uses the Ryan Hipple ScriptableObject architecture. Game state and references are stored in ScriptableObject assets rather than in scene-coupled MonoBehaviours:

   - FloatVariable, Vector3Variable, GameObjectVariable — typed SO wrappers for individual values
   - GameObjectRuntimeSet — a SO-based list that components add and remove themselves from at runtime (e.g., AllEnemies)

  The modifier pattern drives all temporary effects. Rather than polling or calling methods directly on targets, the game dynamically adds a short-lived MonoBehaviour component (the modifier) to the target.
  Each modifier evaluates a condition in a coroutine and destroys itself when the condition is no longer met. The static ModifierLibrary class acts as a factory for all modifier types, with de-duplication
  checks that prevent stacking.

  A Game Manager prefab holds the DamageTypeLibrary singleton and persists across scenes via DontDestroyOnLoad.

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Third-Party Plugins

   - A* Pathfinding Project — navigation and pathfinding for enemies and NPCs
   - Odin Inspector — enhanced Unity editor tooling for inspector customization
   - OpenAI API for Unity — present in the project; likely intended for AI-driven NPC behavior or content generation
   - BreadAI — a behavior AI framework for enemy decision-making, with baked navmesh data in BreadAI/BakedData/

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Technology Stack

   - Unity (C#)
   - Windows standalone build target

  ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Opening the Project

  Open the project in Unity by pointing the Unity Hub at the repository root. The project requires the Unity version compatible with the included ProjectSettings. All plugins are present under 
  Assets/Plugins/ and Packages/.

  To run the existing build without Unity, launch Build/Blob.exe directly on Windows.

 ~     
