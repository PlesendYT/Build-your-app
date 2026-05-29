## Characters for Dating Visual Novel
# Defines romanceable characters, affection tracking, day counter, and image placeholders

define mc = Character("You", color="#ffffff")
define alex = Character("Alex", color="#ff6b6b")
define sam = Character("Sam", color="#4ecdc4")
define jordan = Character("Jordan", color="#45b7d1")
define riley = Character("Riley", color="#f9ca24")
define narrator = Character(None, kind=nvl)

## Relationship tracking
default alex_affection = 0
default sam_affection = 0
default jordan_affection = 0
default riley_affection = 0
default day = 1
default location = "cafe"

## Images (placeholder descriptions)
image bg cafe = "bg_cafe.png"
image bg park = "bg_park.png"
image bg library = "bg_library.png"
image bg beach = "bg_beach.png"
image bg home = "bg_home.png"

image alex happy = "alex_happy.png"
image alex neutral = "alex_neutral.png"
image alex sad = "alex_sad.png"
image sam happy = "sam_happy.png"
image sam neutral = "sam_neutral.png"
image jordan happy = "jordan_happy.png"
image jordan neutral = "jordan_neutral.png"
image riley happy = "riley_happy.png"
image riley neutral = "riley_neutral.png"
