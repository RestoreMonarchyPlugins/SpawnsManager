# SpawnsManager
Modify spawn tables for items, vehicles, zombies, and animals.

## How it works?
When you install the plugin it will generate `SpawnAssets.{MapName}.xml` file in the `SpawnsManager` folder. For example `SpawnAssets.PEI.xml` for PEI map.  
You can modify this file to change spawn tables for items, vehicles, zombies, and animals.  
The plugin overrides existing SpawnAssets with the same ID and GUID with your configuration.

## Commands
- `/rocket reload SpawnsManager` - Reloads the plugin. It will apply your changes in the `SpawnAssets.{MapName}.xml` file.  
Keep in mind that the plugin doesn't de-spawn naturally spawned items, so if you are testing that you need to restart the server or wait for the items to de-spawn.

## Examples
### PEI Police
This is example spawn asset for PEI Police.  
The description indicates that this spawn asset is used for Zombies and natural items spawn in police locations.
```xml
<SpawnAsset Id="93" Name="PEI_Police" Description="Zombie_Police, Item_Police">
    <GUID>ced03c91-16f2-4969-8e4f-a5d504df4f9d</GUID>
    <Tables>
        <Table AssetId="100" Weight="10" Name="Cobra Magazine" />
        <Table AssetId="44" Weight="10" Name="Low Caliber Civilian Ammunition Box" />
        <Table AssetId="224" Weight="10" Name="Police Bottom" />
        <Table AssetId="225" Weight="10" Name="Police Cap" />
        <Table AssetId="223" Weight="10" Name="Police Top" />
        <Table AssetId="99" Weight="10" Name="Cobra" />
        <Table AssetId="1023" Weight="10" Name="Baton" />
        <Table AssetId="1006" Weight="10" Name="Cobra Box" />
        <Table AssetId="10" Weight="10" Name="Police Vest" />
        <Table AssetId="1195" Weight="10" Name="Handcuffs" />
        <Table AssetId="1196" Weight="10" Name="Handcuffs Key" />
        <Table AssetId="1445" Weight="10" Name="Walkie Talkie " />
        <Table AssetId="1044" Weight="10" Name="Civilian Nightvision" />
        <!-- Added Eaglefire and Military Magazine -->
        <Table AssetId="4" Weight="10" Name="Eaglefire" />
        <Table AssetId="6" Weight="10" Name="Military Magazine" />
    </Tables>
</SpawnAsset>
```
### PEI Military Canada
This is spawn asset for vehicles. You can modify it, for example add desert military vehicles.
```xml
<SpawnAsset Id="306" Name="PEI_Military_Canada" Description="Vehicle_Military_Canada">
    <GUID>b389bd2e-3e68-48c2-8a73-2111c405c987</GUID>
    <Tables>
        <Table AssetId="87" Weight="10" Name="Jeep_Forest" />
        <Table AssetId="52" Weight="10" Name="Humvee_Forest" />
        <Table AssetId="51" Weight="10" Name="Ural_Forest" />
        <Table AssetId="53" Weight="10" Name="APC_Forest" />
        <!-- Added military vehicles from Washington map -->
        <Table AssetId="88" Weight="10" Name="Jeep_Desert" />
        <Table AssetId="56" Weight="10" Name="Humvee_Desert" />
        <Table AssetId="55" Weight="10" Name="Ural_Desert" />
        <Table AssetId="57" Weight="10" Name="APC_Desert" />

    </Tables>
</SpawnAsset>
```
### PEI Wild
This is spawn asset for animals. You can modify it, for example add wolf or bear.
```xml
<SpawnAsset Id="329" Name="PEI_Wild" Description="Animal_Wild">
    <GUID>95835e9b-a3bc-4a92-aa4d-79c13c24b4c8</GUID>
    <Tables>
        <Table AssetId="1" Weight="10" Name="Deer" />
        <Table AssetId="4" Weight="10" Name="Pig" />
        <Table AssetId="6" Weight="10" Name="Cow" />
        <!-- Added Wolf and Bear -->
        <Table AssetId="3" Weight="10" Name="Wolf" />
        <Table AssetId="5" Weight="10" Name="Bear" />
    </Tables>
</SpawnAsset>
```
### Resource Pine
This is spawn asset for pine tree. You can for example add Beehive or Queen Bee from More Farming mod.
```xml
<SpawnAsset Id="517" Name="Resource_Pine" Description="Resource_Pine #1, Resource_Pine #2, Resource_Ornament #1 [XMAS]">
    <GUID>2ba46713-4777-4334-919a-cae49acca78d</GUID>
    <Tables>
        <Table AssetId="41" Weight="60" Name="Pine Log" />
        <Table AssetId="42" Weight="40" Name="Pine Stick" />
        <!-- Added Beehive and Queen Bee from More Farming Mod -->
        <Table AssetId="42187" Weight="10" Name="Beehive" />
        <Table AssetId="42193" Weight="10" Name="Queen Bee" />
    </Tables>
</SpawnAsset>
```