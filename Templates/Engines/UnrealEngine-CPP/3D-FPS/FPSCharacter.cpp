// FPSCharacter.cpp
#include "FPSCharacter.h"
#include "FPSWeapon.h"
#include "Camera/CameraComponent.h"
#include "Components/CapsuleComponent.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "EnhancedInputComponent.h"
#include "EnhancedInputSubsystems.h"
#include "Net/UnrealNetwork.h"

AFPSCharacter::AFPSCharacter()
{
    PrimaryActorTick.bCanEverTick = false;

    FirstPersonCamera = CreateDefaultSubobject<UCameraComponent>(TEXT("FirstPersonCamera"));
    FirstPersonCamera->SetupAttachment(GetCapsuleComponent());
    FirstPersonCamera->SetRelativeLocation(FVector(0.0f, 0.0f, 64.0f));
    FirstPersonCamera->bUsePawnControlRotation = true;

    FP_Mesh = CreateDefaultSubobject<USkeletalMeshComponent>(TEXT("FP_Mesh"));
    FP_Mesh->SetupAttachment(FirstPersonCamera);
    FP_Mesh->SetCastShadow(false);
    FP_Mesh->SetOnlyOwnerSee(true);

    GetMesh()->SetOwnerNoSee(true);
    GetCharacterMovement()->MaxWalkSpeed = WalkSpeed;

    CurrentHealth = MaxHealth;
    CurrentArmor = 0.0f;
}

void AFPSCharacter::BeginPlay()
{
    Super::BeginPlay();
    if (HasAuthority() && DefaultWeaponClass)
    {
        ServerSpawnDefaultWeapon();
    }
    if (APlayerController* PC = Cast<APlayerController>(GetController()))
    {
        if (UEnhancedInputLocalPlayerSubsystem* Subsystem = ULocalPlayer::GetSubsystem<UEnhancedInputLocalPlayerSubsystem>(PC->GetLocalPlayer()))
        {
            Subsystem->AddMappingContext(InputMapping, 0);
        }
    }
}

void AFPSCharacter::SetupPlayerInputComponent(UInputComponent* PlayerInputComponent)
{
    Super::SetupPlayerInputComponent(PlayerInputComponent);
    if (UEnhancedInputComponent* EnhancedInput = Cast<UEnhancedInputComponent>(PlayerInputComponent))
    {
        EnhancedInput->BindAction(MoveAction, ETriggerEvent::Triggered, this, &AFPSCharacter::Move);
        EnhancedInput->BindAction(LookAction, ETriggerEvent::Triggered, this, &AFPSCharacter::Look);
        EnhancedInput->BindAction(JumpAction, ETriggerEvent::Started, this, &AFPSCharacter::StartJump);
        EnhancedInput->BindAction(JumpAction, ETriggerEvent::Completed, this, &AFPSCharacter::StopJump);
        EnhancedInput->BindAction(SprintAction, ETriggerEvent::Started, this, &AFPSCharacter::StartSprint);
        EnhancedInput->BindAction(SprintAction, ETriggerEvent::Completed, this, &AFPSCharacter::StopSprint);
        EnhancedInput->BindAction(FireAction, ETriggerEvent::Started, this, &AFPSCharacter::StartFire);
        EnhancedInput->BindAction(FireAction, ETriggerEvent::Completed, this, &AFPSCharacter::StopFire);
        EnhancedInput->BindAction(ReloadAction, ETriggerEvent::Started, this, &AFPSCharacter::ReloadWeapon);
    }
}

void AFPSCharacter::Move(const FInputActionValue& Value)
{
    FVector2D MoveVector = Value.Get<FVector2D>();
    AddMovementInput(GetActorForwardVector(), MoveVector.Y);
    AddMovementInput(GetActorRightVector(), MoveVector.X);
}

void AFPSCharacter::Look(const FInputActionValue& Value)
{
    FVector2D LookVector = Value.Get<FVector2D>();
    AddControllerYawInput(LookVector.X);
    AddControllerPitchInput(LookVector.Y);
}

void AFPSCharacter::StartJump() { Jump(); }
void AFPSCharacter::StopJump() { StopJumping(); }

void AFPSCharacter::StartSprint()
{
    GetCharacterMovement()->MaxWalkSpeed = SprintSpeed;
}

void AFPSCharacter::StopSprint()
{
    GetCharacterMovement()->MaxWalkSpeed = WalkSpeed;
}

void AFPSCharacter::StartFire()
{
    if (CurrentWeapon) CurrentWeapon->StartFire();
}

void AFPSCharacter::StopFire()
{
    if (CurrentWeapon) CurrentWeapon->StopFire();
}

void AFPSCharacter::ReloadWeapon()
{
    if (CurrentWeapon) CurrentWeapon->Reload();
}

void AFPSCharacter::ServerSpawnDefaultWeapon_Implementation()
{
    FActorSpawnParameters SpawnParams;
    SpawnParams.Owner = this;
    SpawnParams.Instigator = this;
    CurrentWeapon = GetWorld()->SpawnActor<AFPSWeapon>(DefaultWeaponClass, FVector::ZeroVector, FRotator::ZeroRotator, SpawnParams);
    if (CurrentWeapon)
    {
        CurrentWeapon->AttachToComponent(FP_Mesh, FAttachmentTransformRules::SnapToTargetNotIncludingScale, TEXT("WeaponSocket"));
        CurrentWeapon->SetOwner(this);
    }
}

float AFPSCharacter::TakeDamage(float Damage, FDamageEvent const& DamageEvent, AController* EventInstigator, AActor* DamageCauser)
{
    float ActualDamage = Super::TakeDamage(Damage, DamageEvent, EventInstigator, DamageCauser);
    if (CurrentArmor > 0.0f)
    {
        float ArmorAbsorb = FMath::Min(CurrentArmor, ActualDamage * 0.5f);
        CurrentArmor -= ArmorAbsorb;
        ActualDamage -= ArmorAbsorb;
    }
    CurrentHealth = FMath::Clamp(CurrentHealth - ActualDamage, 0.0f, MaxHealth);
    if (CurrentHealth <= 0.0f && HasAuthority())
    {
        Destroy();
    }
    return ActualDamage;
}

void AFPSCharacter::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    DOREPLIFETIME(AFPSCharacter, CurrentHealth);
    DOREPLIFETIME(AFPSCharacter, CurrentArmor);
    DOREPLIFETIME(AFPSCharacter, CurrentWeapon);
}
