## Characters for Sci-Fi Visual Novel
# Defines crew, alien, and AI characters plus ship stats: hull, fuel, morale, tech, and trust

define captain = Character("Captain Nova", color="#f39c12")
define officer = Character("Lt. Chen", color="#1abc9c")
define engineer = Character("Chief Vega", color="#e67e22")
define alien = Character("Xyl'thar", color="#8e44ad")
define ai = Character("ARIA", color="#3498db")
define narrator = Character(None, kind=nvl)

default ship_hull = 100
default fuel = 80
default crew_morale = 70
default alien_trust = 0
default tech_level = 1
