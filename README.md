# Tshock-SitemAtHome
A Tshock Plugin that mimics the /Sitem function from the Dark Gaming Terraria server

It allows you to create weapons with custom properties , These are not persistent thru savin
**NOTE : The command may work successfully ,but the newly spawneed weapon may break and spawn unmodified, Try standing in the floor or just running the cmd again, So prefferably CTRL+C your command before sending it**

## Usage
/sitem \"WEAPON NAME\" -Parameter Value -Parameter Value ...
Example
/sitem "Ice blade" -d 64 -sc 9.5 -ua 6 -ut 6
|Parameters|Description|
|---|---|
|-d| set damage (integer)|
|-k| set knockback (decimal)|
|-ua| Animation time (integer)|
|-ut| Usage time (integer)|
|-s| Projectile to shoot (Proj ID)|
|-ss| Projectile speed (integer)|
|-sc| Weapon scale (Decimal/float)|
|-amt| Amount of the item to give (int)|
|-a| Makes an item to be considered as ammo (AmmoID)|
|-uam| Ammo type consummed when shot (AmmoID)|
|-na| Enabling it makes that an ammo type can't go into the ammo slots (0/1)|

## Permissions
|Command|Permission group|Name in code|
|---|---|---|
|/sitem|tshock.item.spawn|Permissions.item|
