# Unity GridCreator Tool

A Unity Editor utility built to make 2D level layout faster and less repetitive. Instead of dragging individual prefabs into the scene and manual-snapping them, this tool adds a custom editor window to paint tiles directly in the Scene View. 

![GridCreator Demo]

## Features
- **Weighted Tile Spawning:** Assign probability weights to prefabs in your palette so grass, rocks, or flowers scatter naturally while painting grids.
- **Color Tinting:** Easily apply custom colors to tiles on placement using color wheel.
- **Fast Grid Tracking:** Uses a C# Dictionary to track grid coordinates for fast placement and cleanup.
- **Undo Support:** Fully integrated with Unity's Undo system so painted or erased tiles revert cleanly.
- **Erasing:** The 'Erase Mode' allows you to delete objects at grid coordinates using The UI Toggle/Shift + Left-Click while painting.

## How to Use
1. Drop GridCreator.cs into any Assets/Editor/ folder in your Unity project.
2. Open the menu at **Tools > Level Editor > GridCreator**.
3. Add your 2D prefabs to the Palette list, hit **START PAINTING**, and click in the Scene View.
