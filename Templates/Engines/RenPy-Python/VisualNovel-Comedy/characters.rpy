## Characters for Comedy Visual Novel
# Defines characters and tracks chaos level and joke count for branching endings

define player = Character("Charlie", color="#ffffff")
define ben = Character("Ben", color="#e74c3c")
define mia = Character("Mia", color="#9b59b6")
define professor = Character("Professor Higgins", color="#2c3e50")
define narrator = Character(None, kind=nvl)

default chaos_level = 0
default joke_counter = 0
