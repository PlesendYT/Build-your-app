// A single puzzle piece in a 2D match-3 grid.
#include "TwoDPuzzlePiece.h"
#include "PaperFlipbookComponent.h"

// Create the sprite component as the root.
ATwoDPuzzlePiece::ATwoDPuzzlePiece()
{
    Sprite = CreateDefaultSubobject<UPaperFlipbookComponent>(TEXT("Sprite"));
    SetRootComponent(Sprite);
}

// Mark this piece as selected or deselected.
void ATwoDPuzzlePiece::SetSelected(bool Selected)
{
    bIsSelected = Selected;
}
