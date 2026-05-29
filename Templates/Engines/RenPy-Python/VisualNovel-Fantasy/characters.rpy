## Characters for Fantasy Visual Novel
# Defines hero, companions, and NPC plus stat tracking for bravery, wisdom, magic, and trust

define hero = Character("Kaelen", color="#e74c3c")
define luna = Character("Luna", color="#9b59b6")
define thorn = Character("Thorn", color="#2ecc71")
define sage = Character("Sage Elara", color="#3498db")
define narrator = Character(None, kind=nvl)

default bravery = 10
default wisdom = 10
default magic = 10
default luna_trust = 0
default thorn_trust = 0
