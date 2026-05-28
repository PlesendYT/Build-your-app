#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "TwoDPuzzleGrid.generated.h"

class ATwoDPuzzlePiece;

USTRUCT()
struct FGridCell
{
    GENERATED_BODY()
    UPROPERTY() ATwoDPuzzlePiece* Piece = nullptr;
    UPROPERTY() bool bIsEmpty = true;
};

UCLASS()
class ATwoDPuzzleGrid : public AActor
{
    GENERATED_BODY()
public:
    ATwoDPuzzleGrid();

    UPROPERTY(EditAnywhere) int32 GridWidth = 6;
    UPROPERTY(EditAnywhere) int32 GridHeight = 6;
    UPROPERTY(EditAnywhere) float CellSize = 100.0f;

    UFUNCTION(BlueprintCallable)
    void InitializeGrid();

    UFUNCTION(BlueprintCallable)
    bool SwapPieces(int32 X1, int32 Y1, int32 X2, int32 Y2);

    UFUNCTION(BlueprintCallable)
    TArray<FIntPoint> FindMatches();

    UFUNCTION(BlueprintCallable)
    void RemoveMatches(TArray<FIntPoint> Matches);

    UFUNCTION(BlueprintCallable)
    void FillEmptyCells();

    UFUNCTION(BlueprintCallable)
    int32 GetScore() const { return Score; }

protected:
    UPROPERTY(EditDefaultsOnly) TSubclassOf<ATwoDPuzzlePiece> PieceClass;
    TArray<TArray<FGridCell>> Grid;
    int32 Score = 0;

    virtual void BeginPlay() override;
};
