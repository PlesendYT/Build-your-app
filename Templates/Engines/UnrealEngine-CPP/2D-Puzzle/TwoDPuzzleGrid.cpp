#include "TwoDPuzzleGrid.h"
#include "TwoDPuzzlePiece.h"
#include "Engine/World.h"

ATwoDPuzzleGrid::ATwoDPuzzleGrid() {}

void ATwoDPuzzleGrid::BeginPlay()
{
    Super::BeginPlay();
    InitializeGrid();
}

void ATwoDPuzzleGrid::InitializeGrid()
{
    Grid.SetNum(GridHeight);
    for (int32 Y = 0; Y < GridHeight; Y++)
    {
        Grid[Y].SetNum(GridWidth);
        for (int32 X = 0; X < GridWidth; X++)
        {
            FVector Location = GetActorLocation() + FVector(X * CellSize, Y * CellSize, 0);
            auto* Piece = GetWorld()->SpawnActor<ATwoDPuzzlePiece>(PieceClass, Location, FRotator::ZeroRotator);
            if (Piece)
            {
                Piece->GridX = X;
                Piece->GridY = Y;
                Piece->PieceType = FMath::RandRange(0, 5);
                Grid[Y][X] = {Piece, false};
            }
        }
    }
}

bool ATwoDPuzzleGrid::SwapPieces(int32 X1, int32 Y1, int32 X2, int32 Y2)
{
    if (!Grid.IsValidIndex(Y1) || !Grid[Y1].IsValidIndex(X1)) return false;
    if (!Grid.IsValidIndex(Y2) || !Grid[Y2].IsValidIndex(X2)) return false;
    Swap(Grid[Y1][X1], Grid[Y2][X2]);
    if (Grid[Y1][X1].Piece)
    {
        Grid[Y1][X1].Piece->GridX = X1;
        Grid[Y1][X1].Piece->GridY = Y1;
    }
    if (Grid[Y2][X2].Piece)
    {
        Grid[Y2][X2].Piece->GridX = X2;
        Grid[Y2][X2].Piece->GridY = Y2;
    }
    return true;
}

TArray<FIntPoint> ATwoDPuzzleGrid::FindMatches()
{
    TArray<FIntPoint> Matches;
    for (int32 Y = 0; Y < GridHeight; Y++)
    {
        for (int32 X = 0; X < GridWidth - 2; X++)
        {
            if (Grid[Y][X].Piece && Grid[Y][X+1].Piece && Grid[Y][X+2].Piece)
            {
                if (Grid[Y][X].Piece->PieceType == Grid[Y][X+1].Piece->PieceType
                    && Grid[Y][X].Piece->PieceType == Grid[Y][X+2].Piece->PieceType)
                {
                    Matches.Add(FIntPoint(X, Y));
                    Matches.Add(FIntPoint(X+1, Y));
                    Matches.Add(FIntPoint(X+2, Y));
                }
            }
        }
    }
    for (int32 X = 0; X < GridWidth; X++)
    {
        for (int32 Y = 0; Y < GridHeight - 2; Y++)
        {
            if (Grid[Y][X].Piece && Grid[Y+1][X].Piece && Grid[Y+2][X].Piece)
            {
                if (Grid[Y][X].Piece->PieceType == Grid[Y+1][X].Piece->PieceType
                    && Grid[Y][X].Piece->PieceType == Grid[Y+2][X].Piece->PieceType)
                {
                    Matches.Add(FIntPoint(X, Y));
                    Matches.Add(FIntPoint(X, Y+1));
                    Matches.Add(FIntPoint(X, Y+2));
                }
            }
        }
    }
    return Matches;
}

void ATwoDPuzzleGrid::RemoveMatches(TArray<FIntPoint> Matches)
{
    for (auto& M : Matches)
    {
        if (Grid[M.Y][M.X].Piece)
        {
            Grid[M.Y][M.X].Piece->Destroy();
            Grid[M.Y][M.X] = {nullptr, true};
            Score += 10;
        }
    }
}

void ATwoDPuzzleGrid::FillEmptyCells()
{
    for (int32 X = 0; X < GridWidth; X++)
    {
        int32 EmptyCount = 0;
        for (int32 Y = GridHeight - 1; Y >= 0; Y--)
        {
            if (Grid[Y][X].bIsEmpty) EmptyCount++;
            else if (EmptyCount > 0)
            {
                Swap(Grid[Y + EmptyCount][X], Grid[Y][X]);
                if (Grid[Y + EmptyCount][X].Piece)
                {
                    Grid[Y + EmptyCount][X].Piece->GridY = Y + EmptyCount;
                }
            }
        }
        for (int32 Y = 0; Y < EmptyCount; Y++)
        {
            FVector Location = GetActorLocation() + FVector(X * CellSize, Y * CellSize, 0);
            auto* Piece = GetWorld()->SpawnActor<ATwoDPuzzlePiece>(PieceClass, Location, FRotator::ZeroRotator);
            if (Piece)
            {
                Piece->GridX = X;
                Piece->GridY = Y;
                Piece->PieceType = FMath::RandRange(0, 5);
                Grid[Y][X] = {Piece, false};
            }
        }
    }
}
