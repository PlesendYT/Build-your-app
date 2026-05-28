#include "TwoDPuzzlePiece.h"
#include "PaperFlipbookComponent.h"

ATwoDPuzzlePiece::ATwoDPuzzlePiece()
{
    Sprite = CreateDefaultSubobject<UPaperFlipbookComponent>(TEXT("Sprite"));
    SetRootComponent(Sprite);
}

void ATwoDPuzzlePiece::SetSelected(bool Selected)
{
    bIsSelected = Selected;
}
