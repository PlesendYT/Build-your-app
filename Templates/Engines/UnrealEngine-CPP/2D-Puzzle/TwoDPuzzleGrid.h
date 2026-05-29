// Header for a 2D match-3 puzzle grid manager.
#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "TwoDPuzzleGrid.generated.h"

class ATwoDPuzzlePiece;

// A single cell in the puzzle grid.
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

    // Fill the grid with random pieces.
    UFUNCTION(BlueprintCallable)
    void InitializeGrid();

    // Swap two pieces and update their grid coordinates.
    UFUNCTION(BlueprintCallable)
    bool SwapPieces(int32 X1, int32 Y1, int32 X2, int32 Y2);

    // Find all matches of 3+ same-type pieces.
    UFUNCTION(BlueprintCallable)
    TArray<FIntPoint> FindMatches();

    // Destroy the matched pieces.
    UFUNCTION(BlueprintCallable)
    void RemoveMatches(TArray<FIntPoint> Matches);

    // Drop existing pieces and spawn new ones to fill empty cells.
    UFUNCTION(BlueprintCallable)
    void FillEmptyCells();

    // Get the current score.
    UFUNCTION(BlueprintCallable)
    int32 GetScore() const { return Score; }

protected:
    UPROPERTY(EditDefaultsOnly) TSubclassOf<ATwoDPuzzlePiece> PieceClass;
    TArray<TArray<FGridCell>> Grid;
    int32 Score = 0;

    virtual void BeginPlay() override;
};
