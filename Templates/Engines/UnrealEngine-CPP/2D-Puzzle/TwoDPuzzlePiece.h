#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "TwoDPuzzlePiece.generated.h"

class UPaperFlipbookComponent;

UCLASS()
class ATwoDPuzzlePiece : public AActor
{
    GENERATED_BODY()
public:
    ATwoDPuzzlePiece();

    UPROPERTY(BlueprintReadWrite) int32 GridX = 0;
    UPROPERTY(BlueprintReadWrite) int32 GridY = 0;
    UPROPERTY(BlueprintReadWrite) int32 PieceType = 0;
    UPROPERTY(BlueprintReadOnly) bool bIsSelected = false;

    UFUNCTION(BlueprintCallable)
    void SetSelected(bool Selected);

protected:
    UPROPERTY(VisibleAnywhere) UPaperFlipbookComponent* Sprite;
};
