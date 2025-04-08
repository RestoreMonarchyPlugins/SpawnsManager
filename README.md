# SpawnsManager
A plugin that lets you modify spawn tables for items, vehicles, zombies, and animals in Unturned.

## How It Works
When installed, SpawnsManager creates a `SpawnAssets.{MapName}.xml` file in the `SpawnsManager` folder (example: `SpawnAssets.PEI.xml` for PEI map).

You can edit this XML file to change what items, vehicles, zombies, and animals spawn in your world. The plugin overrides the game's default spawn settings with your custom configuration.

## Configuration Options
- **Debug** (true/false): Enables detailed logging for troubleshooting.

## Commands
- `/rocket reload SpawnsManager` - Reloads the plugin and applies changes from the XML file.
  
  **Note:** Reloading doesn't remove already spawned items. You'll need to restart the server or wait for items to despawn naturally to see changes to existing items.

- `/checkmap` - Shows all spawn tables for the current map (zombies, items, vehicles, animals).
- `/checkspawn <spawnId> [assetType]` - Shows details about a specific spawn, including item chances.

## Examples

### Police Zone Spawns
This example adds Eaglefire rifles and Military Magazines to police locations:

```xml
<SpawnAsset Id="93" Name="PEI_Police" Description="Zombie_Police, Item_Police">
    <GUID>ced03c91-16f2-4969-8e4f-a5d504df4f9d</GUID>
    <Tables>
        <Table AssetId="100" Weight="10" Name="Cobra Magazine" />
        <Table AssetId="44" Weight="10" Name="Low Caliber Civilian Ammunition Box" />
        <Table AssetId="224" Weight="10" Name="Police Bottom" />
        <!-- Other default police items... -->
        
        <!-- Added military weapons -->
        <Table AssetId="4" Weight="10" Name="Eaglefire" />
        <Table AssetId="6" Weight="10" Name="Military Magazine" />
    </Tables>
</SpawnAsset>
```

### Military Vehicle Spawns
This example adds desert military vehicles to a map that normally only has forest vehicles:

```xml
<SpawnAsset Id="306" Name="PEI_Military_Canada" Description="Vehicle_Military_Canada">
    <GUID>b389bd2e-3e68-48c2-8a73-2111c405c987</GUID>
    <Tables>
        <!-- Default forest vehicles -->
        <Table AssetId="87" Weight="10" Name="Jeep_Forest" />
        <Table AssetId="52" Weight="10" Name="Humvee_Forest" />
        
        <!-- Added desert vehicles -->
        <Table AssetId="88" Weight="10" Name="Jeep_Desert" />
        <Table AssetId="56" Weight="10" Name="Humvee_Desert" />
    </Tables>
</SpawnAsset>
```

### Wildlife Spawns
Add dangerous animals to wilderness areas:

```xml
<SpawnAsset Id="329" Name="PEI_Wild" Description="Animal_Wild">
    <GUID>95835e9b-a3bc-4a92-aa4d-79c13c24b4c8</GUID>
    <Tables>
        <Table AssetId="1" Weight="10" Name="Deer" />
        <Table AssetId="4" Weight="10" Name="Pig" />
        
        <!-- Added dangerous animals -->
        <Table AssetId="3" Weight="10" Name="Wolf" />
        <Table AssetId="5" Weight="10" Name="Bear" />
    </Tables>
</SpawnAsset>
```

### Tree Resource Drops
Add modded items to tree harvesting:

```xml
<SpawnAsset Id="517" Name="Resource_Pine" Description="Resource_Pine">
    <GUID>2ba46713-4777-4334-919a-cae49acca78d</GUID>
    <Tables>
        <Table AssetId="41" Weight="60" Name="Pine Log" />
        <Table AssetId="42" Weight="40" Name="Pine Stick" />
        
        <!-- Added mod items -->
        <Table AssetId="42187" Weight="10" Name="Beehive" />
        <Table AssetId="42193" Weight="10" Name="Queen Bee" />
    </Tables>
</SpawnAsset>
```

## Understanding the XML Format
- **Id**: The spawn table ID number used by the game to identify the spawn table.
- **Name**: Name of the spawn table (doesn't affect functionality, just for readability).
- **Description**: Shows where this spawn table is used (doesn't affect functionality, just for information).
- **GUID**: Unique identifier that has higher priority than ID - the game first checks GUID, then falls back to ID if needed. Don't change this unless you know what you're doing.
- **Tables**: List of items in the spawn table
  - **AssetId**: The item/vehicle/animal ID from the game or mods.
  - **Weight**: Higher numbers mean more common spawns. For example, an item with weight 20 is twice as likely to spawn as an item with weight 10.
  - **Name**: Friendly name (for readability only, doesn't affect functionality).