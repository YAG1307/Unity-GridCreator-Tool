# Unity GridCreator Tool

Instead of manually dragging and positioning individual prefabs, GridCreator tool lets you paint directly into the Scene View using a customizable prefab palette.

[GridCreator Demo](<img width="800" height="393" alt="GridPainter GIF" src="https://github.com/user-attachments/assets/5c98b3fc-7e28-4438-a57d-bdd2a4466a24" />)

## Features
- **Weighted Tile Spawning:** Assign probability weights to prefabs in your palette so grass, rocks, or flowers scatter naturally while painting grids.
- **Color Tinting:** Easily apply custom colors to tiles on placement using color wheel.
- **Fast Grid Tracking:** Uses a C# Dictionary to track grid coordinates for fast placement and cleanup.
- **Undo Support:** Fully integrated with Unity's Undo system so painted or erased tiles revert cleanly.
- **Erasing:** Remove placed tiles by clicking occupied grids, works with both UI toggle and Shift + Left-Click.

## How to Use
1. Drop GridCreator.cs into any Assets/Editor/ folder in your Unity project.
2. Open the menu at **Tools > Level Editor > GridCreator**.
3. Add your 2D prefabs to the Palette list, hit **START PAINTING**, adjust settings (weight cannot be 0), and click in the Scene View.
