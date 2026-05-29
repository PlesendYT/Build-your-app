## Characters for Mystery Visual Novel
# Defines detective, suspects, and tracks clues, interviewed list, and the correct culprit

define detective = Character("Detective Morgan", color="#2c3e50")
define player = Character("You", color="#ffffff")
define suspect1 = Character("Mrs. Blackwood", color="#8e44ad")
define suspect2 = Character("Mr. Sterling", color="#d35400")
define suspect3 = Character("Ms. Chen", color="#2980b9")
define suspect4 = Character("Dr. Reeves", color="#27ae60")
define narrator = Character(None, kind=nvl)

default clues_found = []
default interviewed = []
default accusation_made = False
default correct_suspect = "Ms. Chen"
