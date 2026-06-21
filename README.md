# miraculous-sled-surfers

## Unity version
Untiy 2022.3.62f3

## How to run
- Open the project in Unity.
- Start from the `Preload` scene.
- Press Play.

## Notes
- Tested and developed on Windows targetting android platform
- Visual polish and level design: all the elements present in the gameplay are for showcasing purposes, I prioritized architecture and code cleanness over visual integration. 
- Game balance is still being refined. A cheat/debug menu is included to facilitate testing upgrades and other gameplay features.

# Architecture

## Core Loop

Preload, fetch player data, load level, show main menu with the player visible on the level, aim the slingshot, launch, run, then crash, lose momentum, or reach the end, then results, then back to main menu for a retry or the next level. When a level is completed, upgrades and bound currencies are reset for the next level.

## Scenes

Three scenes, loaded additively: Preload (bootstraps everything then unloads), GameCore (player, camera, managers, all UI, loaded once and kept alive for the whole session), and Level_X (track geometry, obstacles, collectables, player spawn point).

## Architecture Style

Managers own state and lifecycle. Controllers drive one entity's behavior frame by frame. A small ServiceLocator handles cross cutting access. Orchestrators call subsystems directly when order matters. Subsystems only expose events upward, never hold a reference to their orchestrator. A pull based service stores run results so UI can read them whenever it becomes active, instead of needing to catch a live event at the right moment.

## Main managers
- GameManager, in charge of setting up everything once a level is loaded, start/finish game and orchestrate subsystems below
- Gameplay manager is in charge of everything that happens during a run.
- PlayerManager:  a facade over several focused subsystems: launch and physics, ground alignment, lateral tilt, collision detection, momentum tracking, sled visuals, and character visuals. Physics and visuals are fully separate, so the sled and character can be swapped, ragdolled, or detached independently. The Rigidbody is kinematic by default and only goes active for the actual run.
- CurrencyManager: in charge of updating the different currencies and notifying of currency changes
- DataManager: with the help of a data provider, stores and saves player data
- UpgradesManager: provides the method to access the available upgrade values and level them up
- Collectables: levels have markers for collectables instead of their prefabs. Later on, when a level is loaded, a collectable pool reads the info of the markers and instantiates or positions the collectables


## Character visuals and animations

One Animator Controller drives every transition. Different characters reuse the same graph through Animator Override Controllers, swapping only which clip plays per state. Root motion is disabled since the Rigidbody owns position.

## Slingshot

Aiming bounds form a U shape made of two straight sides and a bottom semicircle. Pull percentage comes from distance to the anchor point, not from hardcoded zones. SlingshotManager only reports direction and percentage; PlayerManager converts that into an actual launch force.

## Camera

One camera with four modes: main menu, idle aiming, following, and frozen after a run ends. Menu and aiming views orbit around the player instead of cutting across, and the camera reports when a transition has settled so gameplay systems know when it is safe to enable input.

## Data

Most tunable systems, character physics, slingshot shape, upgrades, and character visuals, are configured through ScriptableObjects rather than scattered serialized fields on components. This keeps tweaking fast during development and leaves the door open to override any of these configs from a remote JSON payload later, without changing how gameplay code reads them.


# Next steps

- Add more juice: all the architecture was thought to easily wire up new subsystems and indicators. A small list of possible additions: add fx and fov changes depending on the speed of the player, add animations to UI transitions, add toasts when a collectble is collected, add visual indicators when launching the player and during the run so player can visualize better its progress, add visual indicator of best distance, add animations using DOTween package.

- Mechanics: add level zones with different friction and visual indicator when going on those zones

- Save data: since current data is small enough it is loaded and saved in batch. In the future if it gets bigger It should be split the saving and retrieving into a more modular approach.

- If it was a server authoritative game, spending currency logic should change to async and wait for a result, or in a more complex way dont wait for the result, register the operation(s) and revert them in case of error

- Loading screen should at least hold info of which system is requesting the load, in case several systems need loading so it hides only only when all systems that triggered it call hide, with a timeout ward to remove a system in case it got destroyed by any reason while waiting to avoid a soft lock. It is currently only used in bootstrap but it should be used anywhere where an async operation happens, like while loading levels.

- Localization: no text is localized, the easiest solution would be to create and attach a component where you put the key and it automatically translates the needed texts.

- Upgrades: for the sake of simplicity, each upgrade can only update one value, although for the slide it could be nice to upgrade several aspects of it besides tilt capacity.

- Create a sprite atlas for the currencies so they can be set in text instead of "coins" or "butterflies"

